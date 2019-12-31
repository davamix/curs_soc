'''
This script read all images from the input folder
and extract the faces detected on them using MTCNN
'''


import torch
from facenet_pytorch import MTCNN, training
from torch.utils.data import DataLoader
from torchvision import datasets, transforms

import os
import sys
import argparse

parser = argparse.ArgumentParser(description='Face detector with MTCNN')
parser.add_argument('source', help='Folder with images, could be a parent folder')
parser.add_argument('--output', '-o', help='Output folder')

args = parser.parse_args()

source_path = args.source
output_path = args.output

# Create ouput_path if not exists
if not os.path.isdir(output_path):
    print(f"Creating output folder {output_path}")
    os.makedirs(output_path)

# Setting GPU or CPU
device = torch.device('cuda:0' if torch.cuda.is_available() else 'cpu')
print(f"Running on {device}")

# Configure detector
mtcnn = MTCNN(
    image_size=224, margin=0, min_face_size=20,
    thresholds=[0.6, 0.7, 0.7], factor=0.709, post_process=True,
    keep_all=True, device=device
)

# Create the dataloader
dataset = datasets.ImageFolder(source_path, transform=transforms.Resize((512, 512)))

# Add output path to samples
dataset.samples = [
    (p, p.replace(source_path, output_path))
        for p, _ in dataset.samples
]

print(f"Total classes: {len(dataset.class_to_idx)}")
print("Classes: ", *dataset.classes, sep='\n -> ', )

#print(dataset.samples)

# Set parameters
batch_size = 8
workers = 0 if os.name == 'nt' else 4

# Create the dataloader
loader = DataLoader(
    dataset,
    num_workers = workers,
    batch_size = batch_size,
    collate_fn = training.collate_pil
)

# Detect
for i, (x, y) in enumerate(loader):
    mtcnn(x, save_path=y)
    print(f"\rBatch {i + 1} of {len(loader)}", end='')

print(f"Faces cropped in {output_path}")