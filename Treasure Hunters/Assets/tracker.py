import board
import pwmio
import time
#from ws2812 import WS2812
import usb_cdc

pwm_servo = pwmio.PWMOut(board.GP20, frequency = 100)

#led = WS2812(16, 1)

servoMin = 4000
servoMax = 16000
diff = servoMax - servoMin

R = 255
G = 150
B = 0

#led.pixels_fill((R, G, B))
#led.pixels_show()

threshold = 10
closeThresh = 3
span = threshold - closeThresh

distance = 100 #arbitrarily high number

def compass(angle):
    servo = angle / 180 * diff + servoMin
    print(servo)
    pwm_servo.duty_cycle = int(servo)

while True:

    try:
        if usb_cdc.data.in_waiting > 0:

            line = usb_cdc.data.readline().strip().decode("utf-8")
            print(line)
            line = line.split()
            distance = (float(line[0]))
            angle = (float(line[1]))
            compass(angle)
            
            if distance > threshold:
                R = 255
                G = 150 - 150 * (distance - threshold) / theshold #scale based on 1 threshold length
                B = 0
            else:
                R = 0 + 255 * (distance - closeThresh) / span
                G = 255 - 105 * (distance - closeThresh) / span
                B = 0
        
#        led.pixels_fill((R, G, B))
#        led.pixels_show()
        
    except Exception as e:
        print("Error:", e)
