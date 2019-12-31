''' 
This script reads the annotation files, train.json and test.json, then copy 
the images from the original path to the output folder.
'''

import os
import sys
import pandas as pd
import json
import argparse
import shutil


parser = argparse.ArgumentParser(description='Parse json annotation files')
parser.add_argument('file', help="File to be parsed")
parser.add_argument('source', help="Source images path")
parser.add_argument('--output', '-o', default='./output', help='Output folder')

args = parser.parse_args()

output_path = args.output
source_path = args.source
input_file = args.file

if not os.path.isfile(input_file):
    print(f"File {input_file} does not exists.")
    sys.exit()

# Create dataframe form json file
annotations_df = pd.read_json(input_file)

def copy_image(src, dst):
    shutil.copy(src, dst)

def set_destination_folder(annotation_image):
    dst_folder = os.path.join(output_path, os.path.basename(os.path.dirname(annotation_image)))

    # Create folder paths if does not exist
    if not os.path.isdir(dst_folder):
        os.makedirs(dst_folder)
    
    return dst_folder

def rename_image(original, idx):
    extension = os.path.splitext(original)[1]
    name = str(idx) + extension

    return name

def get_name(img_path):
    return os.path.basename(img_path)

for idx, annotation in annotations_df.iterrows():
    print("Setting destination folder...", end=" ")
    dst_folder = set_destination_folder(annotation[0])
    print(dst_folder)

    #print("Changing image name...", end=" ")
    #image_name = rename_image(annotations_df[0][0], idx)
    #print(image_name)
    image_name = get_name(annotation[0])

    src_image = os.path.join(source_path, annotation[0])

    dst_path = os.path.join(dst_folder, image_name)

    print(f"Copying from {src_image} --> {dst_path}")
    copy_image(src_image, dst_path)
