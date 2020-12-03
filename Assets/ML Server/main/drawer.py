import cv2
import numpy as np

def my_filled_circle(img, center_coordinates, text):
    # Radius of circle
    radius = 20
    # Blue color in BGR
    color = (255, 0, 0)
    # Line thickness of 2 px
    thickness = 2

    # Using cv2.circle() method
    # Draw a circle with blue line borders of thickness of 2 px
    img = np.array(img)
    font = cv2.FONT_HERSHEY_SIMPLEX


    fontScale = 1
    fontColor = (0, 0, 0)
    lineType = 2

    img = cv2.putText(img, text, center_coordinates, font, fontScale, fontColor, lineType)

    return cv2.circle(img, center_coordinates, radius, color, thickness)