#include "/Applications/Ultraleap Hand Tracking.app/Contents/LeapSDK/include/LeapC.h"
#include <stdio.h>
#include <stdbool.h>
#include <math.h>

// A hand tracking app like Ultraleap Gemini is needed to run this
// Note that the path to LeapC in the include statement may differ per installation

// Testing to ensure that the hand tracking software can meet our needs
// For now, that means being able to detect if the hand is in a certain area for collisions
// and detecting the hand's velocity for movement

int main() {
    // OPENING THE CONNECTION
    LEAP_CONNECTION* connectionHandle;

    printf("%s\n", "CREATING CONNECTION...\n");
    eLeapRS result = LeapCreateConnection(NULL, connectionHandle);

    if (result == 0) {
        printf("%s\n", "SUCCESS\n\n");
    }
    else {
        printf("%s", "!!FAILURE!! Result Code: ");
        printf("%u\n", result);
        return -1;
    }

    printf("%s\n", "OPENING CONNECTION...\n");
    result = LeapOpenConnection(*connectionHandle);

    if (result == 0) {
        printf("%s\n", "SUCCESS\n\n");
    }
    else {
        printf("%s", "!!FAILURE!! Result Code: ");
        printf("%u\n", result);
        return -1;
    }
    // DONE OPENING CONNECTION


    LEAP_CONNECTION_MESSAGE leapMsg;
    unsigned int timeout = 1000;

    // Keep track of positions on previous update to compute velocity
    float prevX, prevY, prevZ;
    prevX = prevY = prevZ = 0;

    while (true) {
        result = LeapPollConnection(*connectionHandle, timeout, &leapMsg);

        if (leapMsg.type == eLeapEventType_Tracking) {
            LEAP_HAND* hands = leapMsg.tracking_event->pHands;
            LEAP_HAND hand = hands[0];

            // For now, using the palm as a single position point for the hand for simplicity
            // More robust detection will be wanted later (and probably provided by Unity)
            LEAP_PALM palm = hand.palm;

            // Get coordinates of palm
            float x = palm.position.x;
            float y = palm.position.y;
            float z = palm.position.z;

            // Compute velocity
            float vx = x - prevX;
            float vy = y - prevY;
            float vz = z - prevZ;

            float velocityMagnitude = sqrt(vx*vx + vy*vy + vz*vz);

            // Update previous coordinates
            prevX = x;
            prevY = y;
            prevZ = z;


            // Define the bounds of a 40x40x40 virtual box centered on 0, 150, 0  (x, y, z)
            // We'll detect whether the palm position is overlapping this box
            float boxX1 = -20;
            float boxY1 = 130;
            float boxZ1 = -20;
            
            float boxX2 = 20;
            float boxY2 = 170;
            float boxZ2 = 20;

            // Detect collision with box
            if (x >= boxX1 && x <= boxX2 && y >= boxY1 && y <= boxY2 && z >= boxZ1 && z <= boxZ2) {
                printf("%s\n", "!!! COLLISION DETECTED !!!");
            }

            // Check if lunge: high enough velocity, and if so what direction
            if (velocityMagnitude >= 4) {
                printf("%s\n", "!!! LUNGE !!!");
                // Here, we would use the velocity vector entries to launch the player,
                // but there is no player in this demo so it's kind of worthless
            }


            // Print position and velocity each update
            printf("%s %f %f %f\n", "HAND POSITION:", x, y, z);
            printf("%s %f %f %f\n", "HAND VELOCITY VECTOR:", vx, vy, vz);
            printf("%s %f\n", "HAND VELOCITY MAGNITUDE:", velocityMagnitude);
        }
    }

    printf("%s", "DONE");

    return 0;
}