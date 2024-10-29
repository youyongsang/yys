import kivy
import random
from kivy.app import App
from kivy.uix.screenmanager import Screen
from kivy.config import Config
from kivy.uix.boxlayout import BoxLayout
from kivy.uix.label import Label
from kivy.uix.button import Button
from kivy.uix.widget import Widget
from kivy.graphics import Color, Rectangle
from kivy.core.window import Window
from kivy.uix.behaviors import ButtonBehavior
from kivy.uix.anchorlayout import AnchorLayout
from kivy.clock import Clock  # Kivy의 Clock을 이용해 딜레이 처리
from typing import Tuple, Any
from kivy.uix.image import Image
import re

fontName_Bold = 'GowunBatang-Bold.ttf'
fontName_Regular = 'GowunBatang-Regular.ttf'


# 기본 16:9 비율 설정 (예: 720x1280)
target_aspect_ratio = 16 / 9

class ColoredBox(Widget):
    def __init__(self, color=(1, 0, 0, 1), **kwargs):  # 기본값은 빨간색
        super(ColoredBox, self).__init__(**kwargs)
        with self.canvas:
            Color(*color)  # 색 설정 (R, G, B, A)
            self.rect = Rectangle(size=self.size, pos=self.pos)
        # 크기와 위치가 변할 때마다 배경을 다시 그려줍니다.
        self.bind(size=self.update_rect, pos=self.update_rect)

    def update_rect(self, *args):
        self.rect.pos = self.pos
        self.rect.size = self.size
class ClickableLabel(ButtonBehavior, Label):
    pass

class GameScreen(Screen):
    ability_stat = {"컴퓨터기술": 0, "체력": 0, "운": 0, "폭식": 0, "지능": 0, "타자": 0, "속독": 0, "성적": 100, "돈" : 3, "집중도" : 3, "멘탈" : 3}
    main = True
    on_choice_able = False
    day = 0
    start = False
    reaction_part = False
    choice = 0
    reaction_line = ""
    file_name = ""
    flag = True  # 리액션 텍스트를 통한 start_automatic_text 실행 구분
    # 플래그를 통해서 리액션 부분만 출력 가능
    save_file_name = ""  # 리액션 텍스트 돌입 시 이전에 읽었던 텍스트 파일의 이름을 저장
    saved_re_position = ""

    def __init__(self, **kwargs):
        super().__init__(**kwargs)

    def build(self):
        print("빌드 실행")
        # 전체 레이아웃 (수직)
        self.main_layout = BoxLayout(orientation='vertical')

        # 가운데 부분의 레이아웃 (가로로 나눔)
        self.middle_layout = BoxLayout(orientation='horizontal', size_hint=(1, 6 / 7))

        # 텍스트 영역 (가로로 7/8)
        self.text_area = ClickableLabel(
            text="",  # 처음에는 빈 텍스트로 시작
            font_size=24,
            size_hint=(7 / 8, 1),  # 가로로 7/8 크기 설정
            font_name=fontName_Regular,
            text_size=(720 * 7 / 8, None),  # 텍스트 크기를 가로 7/8로 제한
            halign='left',  # 텍스트 왼쪽 정렬
            valign='top',  # 텍스트를 상단 정렬
            markup=True,  # 마크업 활성화
        )

        # 텍스트 영역 클릭 시 다음 줄로 이동
        self.text_area.bind(on_press=self.on_click_next_text)

        # 오른쪽 레이아웃 (1/8 공간에 능력창과 진척도창 버튼 추가)
        self.right_layout = BoxLayout(orientation='vertical', size_hint=(1 / 8, 1))

        # 능력창 및 진척도창 버튼 추가, 크기 1/4로 줄임
        self.ability_button = Button(text="능력창", font_name=fontName_Regular, size_hint=(1, 0.15))
        self.progress_button = Button(text="진척도", font_name=fontName_Regular, size_hint=(1, 0.15))

        # 빈 공간을 검은색으로 설정
        self.black_space = ColoredBox(color=(0, 0, 0, 1), size_hint=(1, 0.5))

        # 오른쪽 레이아웃에 버튼과 빈 공간 추가 (버튼은 상단 정렬)
        self.right_layout.add_widget(self.ability_button)
        self.right_layout.add_widget(self.progress_button)
        self.stat_image_layout = BoxLayout(orientation='vertical', size_hint=(1, 0.4))
        self.right_layout.add_widget(self.stat_image_layout)
        self.right_layout.add_widget(self.black_space)

        # 하단 선택지 영역 (수직으로 4개 선택지 배치)
        self.choice_layout = BoxLayout(orientation='vertical', size_hint=(1, 1 / 4))

        # 선택지 버튼들
        self.choice1 = Button(font_name=fontName_Bold, background_normal='', background_down='',
                              background_color=(0, 0, 0, 1))
        self.choice2 = Button(font_name=fontName_Bold, background_normal='', background_down='',
                              background_color=(0, 0, 0, 1))
        self.choice3 = Button(font_name=fontName_Bold, background_normal='', background_down='',
                              background_color=(0, 0, 0, 1))
        self.choice4 = Button(font_name=fontName_Bold, background_normal='', background_down='',
                              background_color=(0, 0, 0, 1))

        # 각 버튼에 이벤트 핸들러 연결
        self.choice1.bind(on_press=self.on_choice)
        self.choice2.bind(on_press=self.on_choice)
        self.choice3.bind(on_press=self.on_choice)
        self.choice4.bind(on_press=self.on_choice)

        # AnchorLayout을 사용하여 버튼을 아래쪽에 추가
        choice_anchor1 = AnchorLayout(anchor_y='bottom')
        choice_anchor1.add_widget(self.choice1)

        choice_anchor2 = AnchorLayout(anchor_y='bottom')
        choice_anchor2.add_widget(self.choice2)

        choice_anchor3 = AnchorLayout(anchor_y='bottom')
        choice_anchor3.add_widget(self.choice3)

        choice_anchor4 = AnchorLayout(anchor_y='bottom')
        choice_anchor4.add_widget(self.choice4)

        # choice_layout에 버튼들을 추가
        self.choice_layout.add_widget(choice_anchor4)
        self.choice_layout.add_widget(choice_anchor3)
        self.choice_layout.add_widget(choice_anchor2)
        self.choice_layout.add_widget(choice_anchor1)

        # 가운데 레이아웃에 텍스트 영역과 오른쪽 하얀색 공간 추가
        self.middle_layout.add_widget(self.text_area)  # 텍스트 영역 (7/8)
        self.middle_layout.add_widget(self.right_layout)  # 오른쪽 능력창, 진척도창, 검은색 공간 (1/8)

        # 메인 레이아웃에 가운데 레이아웃, 선택지 영역 순서대로 추가
        self.main_layout.add_widget(self.middle_layout)  # 가운데 레이아웃 (텍스트 + 오른쪽 버튼 및 빈 공간)
        self.main_layout.add_widget(self.choice_layout)  # 선택지 영역

        # 윈도우 사이즈 변경 이벤트 핸들러 추가
        Window.bind(on_resize=self.adjust_layout)

        self.add_widget(self.main_layout)
        self.update_stat_images()
        # 텍스트 파일 읽기
        self.story_lines = self.read_story_text('start_story.txt').splitlines()
        self.reset_game()

        # 처음 자동으로 텍스트 출력 시작
        self.start_automatic_text()

    def reset_game(self):  #게임을 다시 시작했을 때 변수값을 초기화하는 메소드
        """ 게임 상태를 초기화하는 메서드 """
        self.current_line = 0
        self.day = 0
        self.start = True
        self.main = True
        self.reaction_part = False
        self.choice = 0
        self.reaction_line = ""
        self.file_name = ""
        self.flag = True
        self.save_file_name = ""
        self.saved_re_position = ""
        self.is_waiting_for_click = False
        self.text_area.text = ""
        self.ability_stat = {"컴퓨터기술": 0, "체력": 0, "운": 0, "폭식": 0, "지능": 0, "타자": 0, "속독": 0, "성적": 100, "돈" : 3, "집중도" : 3, "멘탈" : 3}
        self.update_stat_images()

    def on_enter(self):
        # GameScreen에 들어왔을 때 텍스트 출력을 시작합니다.
        self.clear_widgets()
        self.build()
        print("on enter 실행")

    # 윈도우 크기가 변경될 때 비율 조정
    def adjust_layout(self, instance, width, height):
        # 현재 창의 비율 계산
        self.text_area.text_size = (width * 7 / 8, None)
        if width >800:
            self.text_area.font_size = 32
            self.choice1.font_size = 28
            self.choice2.font_size = 28
            self.choice3.font_size = 28
            self.choice4.font_size = 28
        else :
            self.text_area.font_size = 24
            self.choice1.font_size = 22
            self.choice2.font_size = 22
            self.choice3.font_size = 22

    def update_stat_images(self):
        """ 스탯 값에 따라 이미지를 갱신하는 함수 """
        # 기존 이미지 제거
        self.stat_image_layout.clear_widgets()

        # '돈'에 해당하는 돈 이미지
        money_stat = self.ability_stat.get('돈', 0)
        money_layout = BoxLayout(orientation='horizontal', size_hint=(1, None), height=50)
        for _ in range(money_stat):
            money_layout.add_widget(Image(source='./public/image/icon/money.png'))  # 돈 이미지 경로 설정
        self.stat_image_layout.add_widget(money_layout)

        # '집중도'에 해당하는 세모 이미지
        focus_stat = self.ability_stat.get('집중도', 0)
        focus_layout = BoxLayout(orientation='horizontal', size_hint=(1, None), height=50)
        for _ in range(focus_stat):
            focus_layout.add_widget(Image(source='./public/image/icon/pen.png'))  # 연필 이미지 경로 설정
        self.stat_image_layout.add_widget(focus_layout)

        # '멘탈'에 해당하는 하트 이미지
        mental_stat = self.ability_stat.get('멘탈', 0)
        mental_layout = BoxLayout(orientation='horizontal', size_hint=(1, None), height=50)
        for _ in range(mental_stat):
            mental_layout.add_widget(Image(source='./public/image/icon/heart.png'))  # 하트 이미지 경로 설정
        self.stat_image_layout.add_widget(mental_layout)

    # 텍스트 파일에서 내용을 읽어오는 함수
    def read_story_text(self, file_path):
        try:
            with open(file_path, 'r', encoding='utf-8') as file:
                self.file_name = file.name
                return file.read()
        except FileNotFoundError:
            return "스토리 파일을 찾을 수 없습니다."

    # 자동으로 텍스트를 출력하는 함수 이벤트 분기 확인
    def start_automatic_text(self, dt=None):
        if self.current_line < len(self.story_lines):  # 전체 내용 탐색
            line = self.story_lines[self.current_line].strip()  # 한 줄씩 입력받음

            print(self.file_name, self.current_line, line)
            if self.reaction_part:  # 리액션 파트에 돌입했을 경우
                if line.startswith("#") and line == self.reaction_line:  # 내가 원하는 리액션 파트 진입
                    print("내가 원하는 리액션 파트 진입 성공")
                    self.flag = True  # 텍스트 출력 활성화
                    self.current_line += 1  # 다음 줄 탐색
                    line = self.story_lines[self.current_line].strip()
                elif not (self.flag):  # 내가 원하는 리액션 파트가 아닌 경우
                    self.current_line += 1  # 다음 줄 탐색
                    Clock.schedule_once(self.start_automatic_text, 0.01)
            if self.flag:
                if line == "":  # 빈 줄일 경우 클릭 대기
                    print("빈줄 실행")
                    self.is_waiting_for_click = True  # True일 경우 텍스트 화면 클릭시 다음 줄 텍스트가 출력됨
                elif line.startswith("-"):  # 선택지 항목이면 버튼 텍스트로 설정하고 넘김
                    self.is_waiting_for_click = False
                    self.on_choice_able = True
                    self.set_choices_from_story(self.current_line)
                elif line.startswith("#") and not self.reaction_part:  # 첫 번째 글자가 #일때 리액션 파트는 아닐 경우
                    print("랜덤 이벤트 스토리 진입 성공")
                    self.main = False
                    self.reaction_line = line
                    self.load_alternate_story(self.current_line + 1, line)  # 세이브 텍스트 라인 설정 후 이벤트 스토리 or 리액션 파트 진입
                elif line.startswith("#") and line != self.reaction_line:  # 해당 리액션 파트 출력이 끝났을 경우
                    print("리액션 파트 종료")

                    self.reaction_part = False
                    self.story_lines = self.read_story_text(self.save_file_name).splitlines()  # 이전 스토리 파일 호출
                    self.current_line = self.saved_re_position + 1  # 저장된 위치로 돌아감
                    Clock.schedule_once(self.start_automatic_text, 0.5)
                else:
                    # 일반 텍스트는 출력 (한 줄씩)
                    self.text_area.text += line + "\n"
                    self.current_line += 1

                    # 다음 줄을 0.5초 후에 출력
                    Clock.schedule_once(self.start_automatic_text, 0.5)
        elif self.reaction_part:  # 리액션 텍스트에서 출력을 모두 마쳤을 경우 -> 기존 텍스트 파일로 돌아가야함
            self.story_lines = self.read_story_text(self.save_file_name).splitlines()  # 기존 텍스트 파일 호출
            self.current_line = self.saved_re_position + 1  # 저장된 위치로 돌아감
            self.reaction_part = False
            Clock.schedule_once(self.start_automatic_text, 0.5)
        elif (self.start):  # 종료 텍스트 파일이 start스토리인 경우. 1회 실행
            self.story_lines = self.read_story_text('main_story.txt').splitlines()
            self.current_line = 0
            self.start = False
            self.day += 1
            self.text_area.text += f"{self.day}일차입니다.\n"
            Clock.schedule_once(self.start_automatic_text, 0.5)
        elif not (self.main) and self.day < 5:  # 메인 스토리가 아니고 아직 엔딩에 도달하지 않았을 시
            self.story_lines = self.read_story_text('main_story.txt').splitlines()  # 메인 스토리 호출
            self.current_line = self.saved_position + 1  # 저장된 위치로 돌아감
            Clock.schedule_once(self.start_automatic_text, 0.5)
            self.main = True
        elif (self.day == 2):  # 메인 스토리 루트가 3주차 진입 시
            self.day += 1
            self.story_lines = self.read_story_text('middle_story.txt').splitlines()
            self.current_line = 0
            Clock.schedule_once(self.start_automatic_text, 0.5)
        elif (self.day <= 3):  # 메인 스토리 루틴
            self.story_lines = self.read_story_text('main_story.txt').splitlines()
            self.current_line = 0
            self.day += 1
            self.text_area.text += f"{self.day}일차입니다.\n"
            Clock.schedule_once(self.start_automatic_text, 0.5)
        elif (self.day == 4):
            self.story_lines = self.read_story_text('end_story.txt').splitlines()
            self.current_line = 0
            self.day += 1
            Clock.schedule_once(self.start_automatic_text, 0.5)
        elif (self.day == 5):
            self.day += 1
            self.load_ending_branch()
        else:
            self.end_game()

    def set_choices_from_story(self, start_index):
        choices = []
        adjustments = []

        # 선택지 라인들 (`-`로 시작하는 줄)을 추출
        while start_index < len(self.story_lines):
            line = self.story_lines[start_index].strip()

            # `-`로 시작하는 줄은 선택지로 처리
            if line.startswith("-"):
                choice_text = line[1:].strip()  # `-` 기호를 제거한 선택지 텍스트

                # 조건문이 포함된 경우 처리
                if ":" in choice_text and "?" in choice_text:
                    choice_text, adjustment = self.parse_conditional_choice(choice_text)
                    choices.append(choice_text)
                    adjustments.append(adjustment)  # (stat_name, stat_value, operation) 형식의 튜플이 들어감
                else:
                    # 기존 방식으로 `%`나 `&` 이후의 능력치 조정 정보 추출
                    if "%" in choice_text or "&" in choice_text:
                        choice, adjustment = self.parse_choice_adjustment(choice_text)
                        choices.append(choice)
                        adjustments.append(adjustment)
                    else:
                        # 능력치 조정이 없는 선택지 처리
                        choices.append(choice_text)
                        adjustments.append(None)  # 조정 정보가 없는 경우
            else:
                break  # `-`로 시작하지 않으면 종료

            start_index += 1

        # 선택지 버튼 텍스트 설정
        if len(choices) >= 1:
            self.choice1.text = choices[0]
        if len(choices) >= 2:
            self.choice2.text = choices[1]
        if len(choices) >= 3:
            self.choice3.text = choices[2]
        if len(choices) >= 4:
            self.choice4.text = choices[3]

        # 선택지에 대응하는 능력치 조정 리스트 저장
        self.adjustments = adjustments

        # 선택지 이후의 줄을 스토리 출력 시작 위치로 설정
        self.current_line = start_index  # 선택지 이후의 첫 번째 줄로 이동

    def parse_conditional_choice(self, choice_text):
        # ':'와 '?'로 조건문을 나누기
        main_part, conditional_part = choice_text.split(":", 1)  # 분할 횟수 1 참일 때 실행 문장과 나머지로 이루어짐
        condition, else_part = conditional_part.split("?", 1)  # 분할 횟수 1 조건문과 거짓일 때 실행 문장으로 이루어짐

        # 조건문 해석
        stat_name = ''.join([char for char in condition if char.isalpha()])  # 영어 또는 한글만 출력 (변경 스탯)
        stat_value = int(''.join([char for char in condition if char.isdigit()]))  # 숫자만 출력 (변경값)
        operator = ''.join([char for char in condition if not char.isalnum()])  # 영어 또는 한글이 아닌 특수문자인 경우만 출력 (조건문부등호)

        # stat 딕셔너리에서 현재 능력치를 확인
        current_stat_value = self.ability_stat.get(stat_name, 0)  # 키가 존재하지 않을 경우 0을 반환 (0대신 None넣어도 될듯)

        # 조건 비교
        if self.evaluate_condition(current_stat_value, stat_value, operator):
            # 조건이 참이면 main_part를 선택지 텍스트로 사용하고 조정값 추출
            return self.extract_choice_and_adjustment(main_part)
        else:
            # 조건이 거짓이면 else_part를 선택지 텍스트로 사용하고 조정값 추출
            return self.extract_choice_and_adjustment(else_part)

    # 텍스트 파일의 조건문에 대한 판별 함수
    def evaluate_condition(self, current_value, target_value, operator):
        if operator == ">=":
            return current_value >= target_value
        elif operator == "<=":
            return current_value <= target_value
        elif operator == "=":
            return current_value == target_value
        elif operator == ">":
            return current_value > target_value
        elif operator == "<":
            return current_value < target_value
        return False  # 정의되지 않은 연산자일 경우 False 반환

    # 선택지 텍스트 내용과 능력치 조정 내용을 구분하는 함수
    def extract_choice_and_adjustment(self, text):
        if "%" in text or "&" in text:  # 수행 문장에 능력치 조정이 있는 지 판단
            choice, adjustment = self.parse_choice_adjustment(text)  # 있을 경우 능력치 조정
            return choice, adjustment
        else:
            return text, None  # 능력치 조정이 없는 경우 텍스트만 반환

    def parse_choice_adjustment(self, choice_text):
        """
        텍스트에서 여러 능력치 조정을 처리하는 함수
        예: %지능1&체력1 또는 &지능1%체력1 혼합 형태도 처리 가능
        """
        adjustments = []  # 여러 능력치 조정을 담을 리스트

        # 능력치 조정 전의 선택지 텍스트
        choice_part = re.split("[%&]", choice_text)[0].strip()

        # 조정치 부분만 추출하기 (%, & 기준으로 split)
        adjustment_parts = re.findall(r'[%&][가-힣0-9]+', choice_text)
        # %나 &로 시작하는 단어들 구분 추출 ex &지능1%속독1인경우 &나 %을 기준으로 나눠져서 ['&지능1','%속독1']이 된다.

        for part in adjustment_parts:
            sign = '+' if part[0] == '%' else '-'  # %면 +, &면 - (상황에 맞게 변경 가능)
            adjustments.append(self.extract_stat_adjustment(part[1:], sign))  # part[1:]조정 속성(&%)을 제외한 나머지 문장

        return choice_part, adjustments

    def extract_stat_adjustment(self, adjustment_text: str, operation: str) -> Tuple[str, int, str]:
        """
        주어진 능력치 조정 텍스트에서 능력치 이름과 값을 추출하고 조정 정보 반환
        """
        # 능력치 이름만 추출 (한글 또는 영어 알파벳만 사용)
        stat_name = ''.join([char for char in adjustment_text if char.isalpha()])

        # 능력치 값만 추출
        stat_value = ''.join([char for char in adjustment_text if char.isdigit()])

        # 값이 없을 경우 기본값 0 설정
        stat_value = int(stat_value) if stat_value else 0  # 문자형으로 받았으니 int형 변경

        return (stat_name, stat_value, operation)

    # 텍스트 영역을 클릭하면 다음 텍스트 출력 시작
    def on_click_next_text(self, *args):
        print("is_waiting_for_click : ", self.is_waiting_for_click)
        if self.is_waiting_for_click:
            self.is_waiting_for_click = False  # 클릭을 기다리는 상태를 해제
            self.text_area.text = ""  # 텍스트 영역 초기화
            self.current_line += 1
            self.start_automatic_text()

    # 선택지 버튼을 눌렀을 때의 동작 정의
    def on_choice(self, instance):
        stat_text = ""
        if self.on_choice_able and instance.text != "":
            # 선택된 버튼에 맞는 인덱스를 찾고 해당 조정값을 가져옴
            self.select_text = instance.text
            if instance == self.choice1:
                adjustments = self.adjustments[0]  # (stat_name, stat_value, operation) 각 형태을 인자로 가진 배열
                self.choice = 0
            elif instance == self.choice2:
                adjustments = self.adjustments[1]
                self.choice = 1
            elif instance == self.choice3:
                adjustments = self.adjustments[2]
                self.choice = 2
            elif instance == self.choice4:
                adjustments = self.adjustments[3]
                self.choice = 3
            else:
                adjustments = []  # 빈 리스트로 초기화

            # None일 경우 빈 리스트로 처리
            if adjustments is None:
                adjustments = []
            else :
                stat_text += "[color=808080]|[/color] "

            # 여러 능력치 조정 처리
            for adjustment in adjustments:
                if adjustment:
                    stat_name, stat_value, operation = adjustment
                    if stat_name in self.ability_stat:  # stat 딕셔너리에서 해당 능력치 확인
                        if operation == "+":
                            self.ability_stat[stat_name] += stat_value
                            if stat_name in ["돈", "집중도", "멘탈"] and self.ability_stat[stat_name] > 3:
                                #["돈", "집중도", "멘탈"] 스탯이 최대 스탯인 3을 넘을 경우
                                self.ability_stat[stat_name] = 3 #더해져도 최대치 3으로 설정
                            elif stat_name != "성적" : #성적이 아닐 경우에는 능력치 조정 수치가 텍스트에 보임
                                stat_text += f"[color=A5FFC9]{stat_name} {operation}{stat_value}[/color]  "
                        elif operation == "-" and self.ability_stat[stat_name] >= 1:
                            self.ability_stat[stat_name] -= stat_value
                            if stat_name != "성적" :
                                stat_text += f"[color=FFA5A5]{stat_name} {operation}{stat_value}[/color]  "
                        print(f"{stat_name} 능력치가 {operation}{stat_value}만큼 조정되었습니다.")
                    else:
                        print(f"경고: {stat_name} 능력치는 존재하지 않습니다.")

            # 선택지 버튼 텍스트 초기화 (선택 후)
            self.clear_choices()

            # 선택한 내용을 출력 후 이어서 출력
            self.text_area.text = ""
            self.text_area.text += f"[color=808080]{self.select_text}[/color] {stat_text}\n"
            self.clear_choices()
            self.current_line += 1
            self.on_choice_able = False
            self.start_automatic_text()
            self.update_stat_images()
            print(self.ability_stat)

    def clear_choices(self):
        self.choice1.text = ""
        self.choice2.text = ""
        self.choice3.text = ""
        self.choice4.text = ""

    def load_alternate_story(self, saved_position, line):
        if line == "#":
            # a.txt 파일을 읽기 시작
            self.story_lines = self.read_story_text(self.sub_event_story()).splitlines()
            self.current_line = 0  # 새로운 파일의 첫 번째 줄부터 시작
            # 스토리가 끝났을 때 다시 main_story.txt로 돌아감
            self.saved_position = saved_position
            Clock.schedule_once(self.start_automatic_text, 0.5)
        else:
            self.save_file_name = self.file_name  # 리액션 텍스트에 돌입하기 전 기존 텍스트 파일의 이름을 저장
            print("저장된 파일 이름", self.save_file_name)
            self.story_lines = self.read_story_text(self.reaction_text()).splitlines()
            self.current_line = 0  # 새로운 파일의 첫 번째 줄부터 시작
            # 스토리가 끝났을 때 이전 파일로 돌아감
            self.saved_re_position = saved_position
            self.reaction_part = True  # 리액션 파일 진입 확인 변수
            self.flag = False  # 리액션 파일에 내가 원하는 부분이 나오기 전까지 자동 텍스트 출력 패스
            Clock.schedule_once(self.start_automatic_text, 0.5)

    def sub_event_story(self):
        print("진입확인")
        sub_event_list = ["a.txt", "b.txt", "c.txt", "d.txt", "e.txt"]
        num = random.randint(0, 4)
        return "./event_story/" + sub_event_list[num]

    def reaction_text(self):
        print("리액션 진입")
        reaction_list = ["reaction_a.txt", "reaction_b.txt", "reaction_c.txt", "reaction_d.txt"]
        return "./reaction/"+reaction_list[self.choice]

    def load_ending_branch(self):
        print("엔딩 이벤트 실행")
        self.story_lines = self.read_story_text(self.ending_branch_story()).splitlines()
        self.current_line = 0  # 새로운 파일의 첫 번째 줄부터 시작
        Clock.schedule_once(self.start_automatic_text, 0.5)

    def ending_branch_story(self):
        if self.ability_stat["성적"] > 90:
            return "./ending_part/hidden_end.txt"
        elif self.ability_stat["성적"] > 80:
            return "./ending_part/normal_end.txt"
        else:
            return "./ending_part/bad_end.txt"

    def end_game(self):
        app = App.get_running_app()
        app.game_ending('BAD')