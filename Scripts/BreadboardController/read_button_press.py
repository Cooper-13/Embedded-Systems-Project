import socket
import select
import pyautogui
import time

#pyautogui usually waits after a function call; make it not do that
pyautogui.PAUSE = 0

#Time to wait between polls
pollTime = 0.05

HOST = "192.168.7.1"
PORT = 8080

BUTTON_A = "A"
BUTTON_W = "W"
BUTTON_D = "D"
BUTTON_L = "L"
BUTTON_R = "R"

INPUT_A = "a"
INPUT_W = "w"
INPUT_D = "d"
INPUT_L = "l"
INPUT_R = "r"

buttonInputMap = {BUTTON_A : INPUT_A, BUTTON_W: INPUT_W, BUTTON_D: INPUT_D, BUTTON_L: INPUT_L, BUTTON_R: INPUT_R}

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.bind((HOST, PORT))
sock.listen()

connection, address = sock.accept()

while True:
    readyToRead,_, _ = select.select([connection], [], [], 0)

    if len(readyToRead) > 0:
        buf = connection.recv(64)
    else:
        buf = ""
    buf = str(buf)
    print(buf)

    for button in [BUTTON_A, BUTTON_W, BUTTON_D, BUTTON_L, BUTTON_R]:
        if button in buf:
            print(button + " HELD")
            pyautogui.keyDown(buttonInputMap[button])
        else:
            print(button + " RELEASED")
            pyautogui.keyUp(buttonInputMap[button])

    #time.sleep(pollTime)