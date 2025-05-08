import RPi.GPIO as GPIO
import time
import socket

DEBUG = False

# Time to wait after button received
AFTER_INPUT_TIME = 0.01
# Time to wait at end of loop
AFTER_LOOP_TIME = 0.01

HOST = "192.168.7.1"
PORT = 8080

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((HOST, PORT))

# Setup
A_PIN = 7  # Use the GPIO number
W_PIN = 11
D_PIN = 12
L_PIN = 18
R_PIN = 40

GPIO.setmode(GPIO.BOARD)  # BCM = by GPIO number, BOARD = by physical pin number
GPIO.setup(A_PIN, GPIO.IN, pull_up_down=GPIO.PUD_UP)  # Internal pull-up resistor
GPIO.setup(W_PIN, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.setup(D_PIN, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.setup(L_PIN, GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.setup(R_PIN, GPIO.IN, pull_up_down=GPIO.PUD_UP)

def write_to_file(button):
	with open("button_presses.txt", "a") as f:
		f.write(f"{button}\n")

try:
	print("Waiting for button press...")
	lognum = 0
	while True:
		if DEBUG:
			lognum+=1
			print("\n"*50)
			for pin, name in zip([A_PIN, W_PIN, D_PIN, L_PIN, R_PIN], ["A", "W", "D", "L", "R"]):
				print(str(lognum) + "   PIN " + name + ": " + str(GPIO.input(pin)))
			time.sleep(0.05)
		
		if GPIO.input(A_PIN) == GPIO.LOW:  # Button pressed (pull-up logic)
			button = "A"
			sock.send(bytes("A", "utf-8"))
			print(f"{button} Button was pressed!")
			write_to_file(button)
			time.sleep(AFTER_INPUT_TIME)  # Simple debounce
		if GPIO.input(W_PIN) == GPIO.LOW:  # Button pressed (pull-up logic)
			button = "W"
			print(f"{button} Button was pressed!")
			write_to_file(button)
			sock.send(bytes("W", "utf-8"))
			time.sleep(AFTER_INPUT_TIME)  # Simple debounce
		if GPIO.input(D_PIN) == GPIO.LOW:  # Button pressed (pull-up logic)
			button = "D"
			print(f"{button} Button was pressed!")
			write_to_file(button)
			sock.send(bytes("D", "utf-8"))
			time.sleep(AFTER_INPUT_TIME)  # Simple debounce
		if GPIO.input(L_PIN) == GPIO.LOW:  # Button pressed (pull-up logic)
			button = "L"
			print(f"{button} Button was pressed!")
			write_to_file(button)
			sock.send(bytes("L", "utf-8"))
			time.sleep(AFTER_INPUT_TIME)  # Simple debounce
		if GPIO.input(R_PIN) == GPIO.LOW:  # Button pressed (pull-up logic)
			button = "R"
			print(f"{button} Button was pressed!")
			write_to_file(button)
			sock.send(bytes("R", "utf-8"))
			time.sleep(AFTER_INPUT_TIME)  # Simple debounce
		button = None
		# Add a small delay to avoid busy waiting
		time.sleep(AFTER_LOOP_TIME)
		
		# Keep-alive to prevent computer from pausing USB connection
		sock.send(bytes("K", "utf-8"))

except KeyboardInterrupt:
    print("Exiting...")
except Exception as e:
	print("Error: " + str(e))
finally:
    GPIO.cleanup()
