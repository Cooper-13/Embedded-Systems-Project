from picamera2 import Picamera2
import numpy as np
import os
import time

# Open framebuffer (fb1 usually corresponds to the LCD)
fb = os.open('/dev/fb1', os.O_RDWR)

def rgb888_to_bgr565_image(image):
    """
    Convert an RGB888 image (HxWx3) to BGR565 format for the framebuffer.
    """
    # Ensure uint8 input
    image = np.asarray(image, dtype=np.uint8)

    # Extract RGB channels (convert to uint16 for bit shifts)
    r = image[:, :, 0].astype(np.uint16)  # Red channel
    g = image[:, :, 1].astype(np.uint16)  # Green channel
    b = image[:, :, 2].astype(np.uint16)  # Blue channel

    # Convert to 5-6-5 bits for BGR565 format
    red_5   = (r >> 3) & 0x1F  # 5 bits for Red
    green_6 = (g >> 2) & 0x3F  # 6 bits for Green
    blue_5  = (b >> 3) & 0x1F  # 5 bits for Blue

    # Pack into BGR565 (note: Blue first in BGR565 format)
    bgr565 = (blue_5 << 11) | (green_6 << 5) | red_5

    # Convert to little endian 16-bit bytes (properly packed)
    return bgr565.astype('<u2').tobytes()



# Initialize Picamera2
picam2 = Picamera2()

# Set camera resolution to 480x320 (matching the camera sensor) and format to RGB888
config = picam2.create_preview_configuration(main={"size": (480, 320), "format": "RGB888"})
picam2.configure(config)
picam2.start()

try:
    while True:
        # Capture a frame from the camera (480x320 RGB888)
        frame = picam2.capture_array("main")
        
        # Create a test image with one color (red for example)
        #frame = np.zeros((480, 320, 3), dtype=np.uint8)
        #frame[:, :, 0] = 255  # Red channel set to max (255)

        # Convert the RGB888 image to BGR565
        bgr565_data = rgb888_to_bgr565_image(frame)

        # Write the BGR565 data to the framebuffer at position 0
        os.lseek(fb, 0, os.SEEK_SET)
        os.write(fb, bgr565_data)

        # Optional: Control the frame rate (~10 fps)
        time.sleep(0.1)

except KeyboardInterrupt:
    print("Stopping camera to LCD stream...")

finally:
    # Clean up
    picam2.stop()
    os.close(fb)
    print("Closed framebuffer and stopped camera.")
