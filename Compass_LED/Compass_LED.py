#In this project, we will correspond the potentiometer dial to the gradient light of an rgb LED.

from machine import Pin, PWM, ADC #link to Pin, PWM, and Analog components
from ws2812 import WS2812 #RGB handling
import utime #time-related functions

#LED = machine.Pin(16, machine.Pin.OUT) #initializes linked LED component at D16
LED_RGB = WS2812(16, 1) #pin number, led count
pwm_servo = PWM(Pin(20)) #initializes servo at D20 port
pwm_servo.freq(100)

servoMin = 4000
servoMax = 17000
diff = servoMax - servoMin

num_bucket = 2 #number of color buckets
bucket = dialMax/num_bucket #bucket size

R = 0
G = 0
B = 0

while True:
    dist = nearestTreasure()
    
    #split into 2 buckets:
    if(dist / bucket < 1): #Red to Yellow
        R = 255
        G = 0
        B = 0
        
        G += int(dist/bucket * 150)
        print("red")
    elif(dist / (dialMax/num_bucket) < 2): #Yellow to Green
        R = 255
        G = 150
        B = 0
        
        R -= int((dist - bucket) / bucket * 255)
        G += int((dist - bucket) / bucket * 105)
        print("green")

    color = (R, G, B)
    print(dist)
    print(color)
    LED_RGB.pixels_fill(color)
    LED_RGB.pixels_show()

    theta = treasureAngle()
    theta = (theta + 180) / 360 * diff + 4000 #convert angle value to servo duty cycle
    pwm_servo.duty_u16(int(theta))
    
def nearestTreasure():
    #reads distance to nearest treasure in unity
    return dist

def treasureAngle():
    #reads angle to nearest treasure in unity (can be calculated using trig functions and coordinates if not available)
    return theta
    



