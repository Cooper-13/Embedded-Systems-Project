import serial
import time
import math
import os

# Replace this with the correct port for your Pico
# On Windows, it might be 'COM3' or 'COM4'
# On Mac/Linux, something like '/dev/ttyACM0'
PORT = '/dev/tty.usbmodem1303'  # ← CHANGE THIS!
BAUD = 115200


def getAngle(x,y): 
    if(x == 0):
        if(y >= 0):
            theta = 90
        else:
            theta = 270
    else:
        tan = y / x
        theta = math.atan(tan)
        theta = math.degrees(theta)
        theta = abs(theta)
    
    complement = 90 - theta

    #print ("Before_player: " + str(theta))

    print("Player: " + str(playerX) + " " + str(playerY) + " " + str(playerAng))
    print("Star: " + str(x) + " " + str(y))

    if(x >= 0 and y >= 0): #Q1
        theta = 90 + theta - playerAng
        print("Q1")
    elif(x < 0 and y >= 0): #Q2
        theta = 180 + complement - playerAng
        print("Q2")
    elif(x <= 0 and y < 0): #Q3
        theta = 270 - playerAng + theta
        print("Q3")
    elif(x > 0 and y < 0): #Q4
        theta = 360 - playerAng + complement
        print("Q4")

    #print ("After_player: " + str(theta))

    theta = theta % 360 # Normalize angle to [0, 360)
    if(theta > 180):
        if(theta > 270):
            theta = 0
        else:
            theta = 180
    elif(theta < 0):
        if(theta < -90):
            theta = 0
        else:
            theta = 180
    
    return theta

# Open serial connection
with serial.Serial(PORT, BAUD, timeout=1) as ser:
    time.sleep(2)  # Wait for Pico to reboot/reset after opening serial

    Star1 = open("starPosition1.txt", "r") 
    Star2 = open("starPosition2.txt", "r") 
    Star3 = open("starPosition3.txt", "r") 
    Star4 = open("starPosition4.txt", "r") 
    Star5 = open("starPosition5.txt", "r")
    file = open("playerPosition.txt", "r")
    while True:
        star1 = Star1.readline()
        #star1.split(sep=' ')
        
        star2 = Star2.readline()
        #star2.split(sep=' ')
        
        star3 = Star3.readline()
        #star3.split(sep=' ')
        
        star4 = Star4.readline()
        #star4.split(sep=' ')
        
        star5 = Star5.readline()
        #star5.split(sep=' ')
         
        player = file.read()
        #player.split(sep=' ')

        star1 = list(map(float, star1.split()))
        star2 = list(map(float, star2.split()))
        star3 = list(map(float, star3.split()))
        star4 = list(map(float, star4.split()))
        star5 = list(map(float, star5.split()))
        player = list(map(float, player.split()))

        #print("Star1: ", star1)
        #print("Star2: ", star2)
        #print("Star3: ", star3)
        #print("Star4: ", star4)
        #print("Star5: ", star5)
        print("Player: ", player)

        if(star1 == [] or star2 == [] or star3 == [] or star4 == [] or star5 == [] or player == []):
            print("Star data is empty.")
            continue
        
        playerX = player[0]
        playerY = player[1]
        playerAng = player[3]
        playerAng = playerAng % 360

        distance = None
        
        stars = [star1, star2, star3, star4, star5]
        
        for star in stars:
            currX = float(star[0])
            currY = float(star[1])
            currD = math.sqrt((playerX - currX)**2 + (playerY - currY)**2)#calculate distance to player

            #print("Distance: ", currD)
            if(distance == None or currD < distance):
                distance = currD
                angle = getAngle(currX-playerX, currY-playerY) #calculate angle to player
                
                if(angle > 180):
                    if(angle > 270):
                        angle = 0
                    else:
                        angle = 180
                
            #print("angle: ", angle)
        print(f"{angle}\n".encode())
        ser.write(f"{distance} {angle}\n".encode())
        print("Angle sent to Pico: ", angle)
        #ser.flush()
        time.sleep(0.1)  # Adjust the sleep time as needed
        # Close the files after use

    Star1.close()
    Star2.close()
    Star3.close()
    Star4.close()
    Star5.close()
    file.close()

    ser.close()
    print("Serial connection closed.")
    # Note: The serial connection will be closed automatically when exiting the 'with' block.