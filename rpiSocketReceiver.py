import socket
import select
import pyautogui
import time

# pyautogui usually waits after a function call; make it not do that, since it causes input latency
pyautogui.PAUSE = 0
# Failsafe triggers if mouse is in one corner of the screen
# We have no mouse control, so can safely disable this
pyautogui.FAILSAFE = False

HOST = "192.168.7.1"
PORT = 8080

# Buttons are identified like this in the socket data
BUTTON_A = "A"
BUTTON_W = "W"
BUTTON_D = "D"
BUTTON_L = "L"
BUTTON_R = "R"

# This is the key that is pressed when we see the corresponding button from the socket
INPUT_A = "a"
INPUT_W = "w"
INPUT_D = "d"
INPUT_L = "j"
INPUT_R = "k"

buttonInputMap = {BUTTON_A : INPUT_A, BUTTON_W: INPUT_W, BUTTON_D: INPUT_D, BUTTON_L: INPUT_L, BUTTON_R: INPUT_R}

# Socket setup; this is how we listen to the raspberry pi
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.bind((HOST, PORT))
sock.listen()
connection, address = sock.accept()

while True:
    # Check if there's data to read to prevent socket from waiting, which then prevents us from releasing keys
    readyToRead, _, _ = select.select([connection], [], [], 0)

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