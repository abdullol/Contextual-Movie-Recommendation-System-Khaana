import csv
import sys

arg1 = 2
arg2 = 7
arg3 = 3

li = [arg1, arg2, arg3]
print(li)
csvfile = open(r'D:/Work/Movie_Recommendation_System/mrs/ratings.csv','a', newline = '', encoding = 'cp1252')
writer = csv.writer(csvfile)
writer.writerow(li)