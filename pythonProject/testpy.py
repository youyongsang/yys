import kivy
import random
from kivy.config import Config
from kivy.app import App
from kivy.uix.boxlayout import BoxLayout
from kivy.uix.label import Label
from kivy.uix.button import Button
from kivy.uix.widget import Widget
from kivy.graphics import Color, Rectangle
from kivy.core.window import Window
from kivy.uix.behaviors import ButtonBehavior
from kivy.uix.anchorlayout import AnchorLayout
from kivy.clock import Clock  # Kivy의 Clock을 이용해 딜레이 처리

# 폰트 설정
fontName_Bold = 'GowunBatang-Bold.ttf'
fontName_Regular = 'GowunBatang-Regular.ttf'


# 기본 16:9 비율 설정 (예: 720x1280)
target_aspect_ratio = 16 / 9


# 배경색을 지정하는 클래스
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


# ButtonBehavior를 상속하여 Label을 클릭할 수 있도록 변경
class ClickableLabel(ButtonBehavior, Label):
    pass


class TextGameApp(App):
    stat = {"컴퓨터기술":0,"체력":0,"운":0,"폭식":0,"지능":0,"타자":0, "속독":0,"성적":100}
    main = True
    on_choice_able = False
    day = 0
    start = False
    def build(self):
        # 전체 레이아웃 (수직)
        self.main_layout = BoxLayout(orientation='vertical')

        # 상단에 빨간색 공간 추가 (세로로 1/10)
        self.red_space = ColoredBox(color=(1, 0, 0, 1), size_hint=(1, 1 / 10))

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
            markup=True  # 마크업 활성화
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
        self.right_layout.add_widget(self.black_space)

        # 하단 선택지 영역 (수직으로 4개 선택지 배치)
        self.choice_layout = BoxLayout(orientation='vertical', size_hint=(1, 1 / 4))

        # 선택지 버튼들
        self.choice1 = Button(font_name=fontName_Bold, background_normal='', background_down='', background_color=(0, 0, 0, 1))
        self.choice2 = Button(font_name=fontName_Bold, background_normal='', background_down='', background_color=(0, 0, 0, 1))
        self.choice3 = Button(font_name=fontName_Bold, background_normal='', background_down='', background_color=(0, 0, 0, 1))
        self.choice4 = Button(font_name=fontName_Bold, background_normal='', background_down='', background_color=(0, 0, 0, 1))

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

        # 메인 레이아웃에 빨간색 공간, 가운데 레이아웃, 선택지 영역 순서대로 추가
        self.main_layout.add_widget(self.red_space)  # 빨간색 공간
        self.main_layout.add_widget(self.middle_layout)  # 가운데 레이아웃 (텍스트 + 오른쪽 버튼 및 빈 공간)
        self.main_layout.add_widget(self.choice_layout)  # 선택지 영역

        # 윈도우 사이즈 변경 이벤트 핸들러 추가
        Window.bind(on_resize=self.adjust_layout)

        # 텍스트 파일 읽기
        self.story_lines = self.read_story_text('start_story.txt').splitlines()
        self.current_line = 0
        self.is_waiting_for_click = False  # 클릭을 기다리는 상태 여부
        self.start = True

        # 처음 자동으로 텍스트 출력 시작
        self.start_automatic_text()

        return self.main_layout

    # 윈도우 크기가 변경될 때 비율 조정
    def adjust_layout(self, instance, width, height):
        # 현재 창의 비율 계산
        current_aspect_ratio = width / height

        # 16:9 비율에 맞추어 레이아웃 조정
        if current_aspect_ratio > target_aspect_ratio:
            # 창이 더 넓을 때: 세로를 기준으로 맞춤
            adjusted_width = height * target_aspect_ratio
            Window.size = (adjusted_width, height)
        else:
            # 창이 더 좁을 때: 가로를 기준으로 맞춤
            adjusted_height = width / target_aspect_ratio
            Window.size = (width, adjusted_height)

    # 텍스트 파일에서 내용을 읽어오는 함수
    def read_story_text(self, file_path):
        print("오류확인 위치:read_story_text함수")
        print(file_path)
        try:
            with open(file_path, 'r', encoding='utf-8') as file:
                return file.read()
        except FileNotFoundError:
            return "스토리 파일을 찾을 수 없습니다."

    # 자동으로 텍스트를 출력하는 함수 이벤트 분기 확인
    def start_automatic_text(self, dt=None):
        if self.current_line < len(self.story_lines): #전체 내용 탐색
            line = self.story_lines[self.current_line].strip() #한 줄씩 입력받음
            if line == "":  # 빈 줄일 경우 클릭 대기
                print("클릭대기상태")
                self.is_waiting_for_click = True #True일 경우 텍스트 화면 클릭시 다음 줄 텍스트가 출력됨
            elif line.startswith("-"):  # 선택지 항목이면 버튼 텍스트로 설정하고 넘김
                self.is_waiting_for_click = False
                self.on_choice_able = True
                self.set_choices_from_story(self.current_line)
            elif line.startswith("#"):  # 첫 번째 글자가 #이면 다른 파일 읽기
                self.main = False
                self.load_alternate_story(self.current_line + 1)  # 세이브 텍스트 라인 설정 후 이벤트 스토리 진입
            else:
                # 일반 텍스트는 출력 (한 줄씩)
                self.text_area.text += line + "\n"
                self.current_line += 1

                # 다음 줄을 0.5초 후에 출력
                Clock.schedule_once(self.start_automatic_text, 0.5)
        elif (self.start): #start스토리인 경우. 1회 실행
            self.story_lines = self.read_story_text('main_story.txt').splitlines()
            self.current_line = 0
            self.start = False
            self.day += 1
            print(self.stat)
            self.text_area.text += f"{self.day}일차입니다.\n"
            Clock.schedule_once(self.start_automatic_text, 0.5)
        elif not(self.main): # 메인 스토리가 아니라면
            self.story_lines = self.read_story_text('main_story.txt').splitlines() # 메인 스토리 호출
            self.current_line = self.saved_position + 1  # 저장된 위치로 돌아감
            Clock.schedule_once(self.start_automatic_text, 0.5)
            self.main = True
        elif(self.day == 2): # 메인 스토리 루트가 3주차 진입 시
            self.day += 1
            self.story_lines = self.read_story_text('middle_story.txt').splitlines()
            self.current_line = 0
            Clock.schedule_once(self.start_automatic_text, 0.5)
        elif(self.day<=3): # 메인 스토리 루틴
            print("메인 스토리 루트 진입")
            self.story_lines = self.read_story_text('main_story.txt').splitlines()
            self.current_line = 0
            self.day += 1
            self.text_area.text += f"{self.day}일차입니다.\n"
            Clock.schedule_once(self.start_automatic_text, 0.5)
        elif(self.day == 4):
            print("엔딩 스토리 진입")
            self.story_lines = self.read_story_text('end_story.txt').splitlines()
            self.current_line = 0
            self.day += 1
            Clock.schedule_once(self.start_automatic_text, 0.5)
        elif(self.day == 5):
            self.day += 1
            self.load_ending_branch()
        else:
            exit(1)

    def set_choices_from_story(self, start_index):
        choices = []
        adjustments = []

        # 선택지 라인들 (`-`로 시작하는 줄)을 추출
        while start_index < len(self.story_lines):
            line = self.story_lines[start_index].strip()

            # `-`로 시작하는 줄은 선택지로 처리
            if line.startswith("-"):
                choice_text = line[1:].strip()  # `-` 기호를 제거한 선택지 텍스트

                # 선택지 텍스트에서 `%`나 `&` 이후의 능력치 조정 정보 추출
                if "%" in choice_text or "&" in choice_text:
                    choice, adjustment = self.parse_choice_adjustment(choice_text)
                    choices.append(choice)
                    adjustments.append(adjustment)
                else:
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

    def parse_choice_adjustment(self, choice_text):
        # `%` 또는 `&` 기호로 구분하여 능력치 정보를 추출
        if "%" in choice_text:
            choice, adjustment = choice_text.split("%", 1)
            operation = "+"
        elif "&" in choice_text:
            choice, adjustment = choice_text.split("&", 1)
            operation = "-"
        else:
            return choice_text, None

        # adjustment 문자열에서 능력치 항목과 조정값을 추출
        stat_name = ''.join([char for char in adjustment if char.isalpha()])
        stat_value = int(''.join([char for char in adjustment if char.isdigit()]))

        return choice.strip(), (stat_name, stat_value, operation)

    # 텍스트 영역을 클릭하면 다음 텍스트 출력 시작
    def on_click_next_text(self, *args):
        print("is_waiting_for_click : ", self.is_waiting_for_click)
        if self.is_waiting_for_click:
            self.is_waiting_for_click = False  # 클릭을 기다리는 상태를 해제
            self.text_area.text = ""  # 텍스트 영역 초기화

            # 현재 줄이 빈 줄이라면 건너뜀
            while self.current_line < len(self.story_lines):
                line = self.story_lines[self.current_line].strip()
                self.current_line += 1

                if line == "":  # 여전히 빈 줄이면 다음 줄로 넘어감
                    continue

                # 유효한 텍스트를 발견하면 텍스트 업데이트 후 자동 출력 시작
                self.text_area.text += line + "\n"
                break

            self.start_automatic_text()

    # 선택지 버튼을 눌렀을 때의 동작 정의
    def on_choice(self, instance):
        print(instance.text == "")
        if self.on_choice_able and instance.text != "": #버튼 메세지 존재 시만 활성화
            print("버튼 활성화")
            # 선택된 버튼에 맞는 인덱스를 찾고 해당 조정값을 가져옴
            self.select_text = instance.text
            if instance == self.choice1:
                adjustment = self.adjustments[0]
            elif instance == self.choice2:
                adjustment = self.adjustments[1]
            elif instance == self.choice3:
                adjustment = self.adjustments[2]
            elif instance == self.choice4:
                adjustment = self.adjustments[3]
            else:
                selected_choice_text = ""
                adjustment = None

            # 선택지 출력 후 능력치 조정
            if adjustment:
                stat_name, stat_value, operation = adjustment
                if operation == "+":
                    self.stat[stat_name] += stat_value
                elif operation == "-":
                    self.stat[stat_name] -= stat_value

            # 선택지 버튼 텍스트 초기화 (선택 후)
            self.clear_choices()

            # 일단 선택한 내용을 출력 후 이어서 출력
            self.text_area.text = ""
            self.text_area.text += f"[color=808080]{self.select_text}[/color]\n"
            self.clear_choices()
            self.current_line += 1
            self.on_choice_able = False
            self.start_automatic_text()

    def clear_choices(self):
        self.choice1.text = ""
        self.choice2.text = ""
        self.choice3.text = ""
        self.choice4.text = ""

    def load_alternate_story(self, saved_position):
        # a.txt 파일을 읽기 시작
        print("오류발생구간?")
        self.story_lines = self.read_story_text(self.sub_event_story()).splitlines()
        self.current_line = 0  # 새로운 파일의 첫 번째 줄부터 시작
        # 스토리가 끝났을 때 다시 main_story.txt로 돌아감
        self.saved_position = saved_position
        Clock.schedule_once(self.start_automatic_text, 0.5)

    def sub_event_story(self):
        print("진입확인")
        sub_event_list = ["a.txt", "b.txt", "c.txt", "d.txt", "e.txt"]
        num = random.randint(0, 4)
        return sub_event_list[num]

    def load_ending_branch(self):
        self.story_lines = self.read_story_text(self.ending_branch_story()).splitlines()
        self.current_line = 0  # 새로운 파일의 첫 번째 줄부터 시작
        print(self.stat)
        Clock.schedule_once(self.start_automatic_text, 0.5)

    def ending_branch_story(self):
        if self.stat["성적"]>90:
            return "hidden_end.txt"
        elif self.stat["성적"]>80:
            return "normal_end.txt"
        else:
            return "bad_end.txt"

if __name__ == '__main__':
    TextGameApp().run()
