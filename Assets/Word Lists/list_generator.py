#original source file for all words can be found at https://github.com/dwyl/english-words/blob/master/words_alpha.txt
from copy import copy

#file names
all_words_file_name = 'all words.txt'
all_words_no_swears_file_name = 'all words NP.txt' #NP stands for no profanities
swears_to_remove_file_name = 'input - profanities to remove.txt'
words_to_add_file_name = 'input - words to add.txt'
words_to_remove_file_name = 'input - words to remove.txt'
words_3_letter_no_swears_file_name = '3-letter words NP.txt'
words_4_letter_no_swears_file_name = '4-letter words NP.txt'
words_5_letter_no_swears_file_name = '5-letter words NP.txt'
words_6_letter_no_swears_file_name = '6-letter words NP.txt'
words_7_letter_no_swears_file_name = '7-letter words NP.txt'
words_8_letter_no_swears_file_name = '8-letter words NP.txt'
words_9_letter_no_swears_file_name = '9-letter words NP.txt'
words_10_letter_no_swears_file_name = '10-letter words NP.txt'
words_11_letter_no_swears_file_name = '11-letter words NP.txt'
words_12_letter_no_swears_file_name = '12-letter words NP.txt'
words_13_letter_no_swears_file_name = '13-letter words NP.txt'
words_14_letter_no_swears_file_name = '14-letter words NP.txt'
words_15_letter_no_swears_file_name = '15-letter words NP.txt'
words_3_letter_file_name = '3-letter words.txt'
words_4_letter_file_name = '4-letter words.txt'
words_5_letter_file_name = '5-letter words.txt'
words_6_letter_file_name = '6-letter words.txt'
words_7_letter_file_name = '7-letter words.txt'
words_8_letter_file_name = '8-letter words.txt'
words_9_letter_file_name = '9-letter words.txt'
words_10_letter_file_name = '10-letter words.txt'
words_11_letter_file_name = '11-letter words.txt'
words_12_letter_file_name = '12-letter words.txt'
words_13_letter_file_name = '13-letter words.txt'
words_14_letter_file_name = '14-letter words.txt'
words_15_letter_file_name = '15-letter words.txt'

#read in input files
all_words_list = open(all_words_file_name).read().splitlines()
all_words_no_swears_list  = open(all_words_no_swears_file_name).read().splitlines()
swears_to_remove = open(swears_to_remove_file_name).read().splitlines()
words_to_add = open(words_to_add_file_name).read().splitlines()
words_to_remove = open(words_to_remove_file_name).read().splitlines()

#update list of all words and NP words
new_words_list = copy(all_words_list)
new_words_list_no_swears = copy(all_words_no_swears_list)
for word in words_to_add:
    new_words_list.append(word)
    new_words_list_no_swears.append(word)
for word in words_to_remove:
    if word in all_words_list:
        new_words_list.remove(word)
    if word in all_words_no_swears_list:
        new_words_list_no_swears.remove(word)
for word in swears_to_remove:
    if word in new_words_list:
        new_words_list_no_swears.remove(word)
new_words_list.sort()
new_words_list_no_swears.sort()
with open(all_words_file_name, 'w') as file:
    for word in new_words_list:
        file.write(f"{word}\n")
with open(all_words_no_swears_file_name, 'w') as file:
    for word in new_words_list_no_swears:
        file.write(f"{word}\n")
        
#clear the to-add and to-remove and swears files
with open(words_to_add_file_name, "w") as file:
    pass
with open(words_to_remove_file_name, "w") as file:
    pass
with open(swears_to_remove_file_name, "w") as file:
    pass
        
#split word list by length
words_3 = []
words_4 = []
words_5 = []
words_6 = []
words_7 = []
words_8 = []
words_9 = []
words_10 = []
words_11 = []
words_12 = []
words_13 = []
words_14 = []
words_15 = []
for word in new_words_list:
    if (len(word) == 3):
        words_3.append(word)
    if (len(word) == 4):
        words_4.append(word)
    if (len(word) == 5):
        words_5.append(word)
    if (len(word) == 6):
        words_6.append(word)
    if (len(word) == 7):
        words_7.append(word)
    if (len(word) == 8):
        words_8.append(word)
    if (len(word) == 9):
        words_9.append(word)
    if (len(word) == 10):
        words_10.append(word)
    if (len(word) == 11):
        words_11.append(word)
    if (len(word) == 12):
        words_12.append(word)
    if (len(word) == 13):
        words_13.append(word)
    if (len(word) == 14):
        words_14.append(word)
    if (len(word) == 15):
        words_15.append(word)

#write word lengths to their respective files
with open(words_3_letter_file_name, 'w') as file:
    for word in words_3:
        file.write(f"{word}\n")
with open(words_4_letter_file_name, 'w') as file:
    for word in words_4:
        file.write(f"{word}\n")
with open(words_5_letter_file_name, 'w') as file:
    for word in words_5:
        file.write(f"{word}\n")
with open(words_6_letter_file_name, 'w') as file:
    for word in words_6:
        file.write(f"{word}\n")
with open(words_7_letter_file_name, 'w') as file:
    for word in words_7:
        file.write(f"{word}\n")
with open(words_8_letter_file_name, 'w') as file:
    for word in words_8:
        file.write(f"{word}\n")
with open(words_9_letter_file_name, 'w') as file:
    for word in words_9:
        file.write(f"{word}\n")
with open(words_10_letter_file_name, 'w') as file:
    for word in words_10:
        file.write(f"{word}\n")
with open(words_11_letter_file_name, 'w') as file:
    for word in words_11:
        file.write(f"{word}\n")
with open(words_12_letter_file_name, 'w') as file:
    for word in words_12:
        file.write(f"{word}\n")
with open(words_13_letter_file_name, 'w') as file:
    for word in words_13:
        file.write(f"{word}\n")
with open(words_14_letter_file_name, 'w') as file:
    for word in words_14:
        file.write(f"{word}\n")
with open(words_15_letter_file_name, 'w') as file:
    for word in words_15:
        file.write(f"{word}\n")
        
#split swear word list by length
words_3_no_swears = []
words_4_no_swears = []
words_5_no_swears = []
words_6_no_swears = []
words_7_no_swears = []
words_8_no_swears = []
words_9_no_swears = []
words_10_no_swears = []
words_11_no_swears = []
words_12_no_swears = []
words_13_no_swears = []
words_14_no_swears = []
words_15_no_swears = []
for word in new_words_list_no_swears:
    if (len(word) == 3):
        words_3_no_swears.append(word)
    if (len(word) == 4):
        words_4_no_swears.append(word)
    if (len(word) == 5):
        words_5_no_swears.append(word)
    if (len(word) == 6):
        words_6_no_swears.append(word)
    if (len(word) == 7):
        words_7_no_swears.append(word)
    if (len(word) == 8):
        words_8_no_swears.append(word)
    if (len(word) == 9):
        words_9_no_swears.append(word)
    if (len(word) == 10):
        words_10_no_swears.append(word)
    if (len(word) == 11):
        words_11_no_swears.append(word)
    if (len(word) == 12):
        words_12_no_swears.append(word)
    if (len(word) == 13):
        words_13_no_swears.append(word)
    if (len(word) == 14):
        words_14_no_swears.append(word)
    if (len(word) == 15):
        words_15_no_swears.append(word)

#write swear word lengths to their respective files
with open(words_3_letter_no_swears_file_name, 'w') as file:
    for word in words_3_no_swears:
        file.write(f"{word}\n")
with open(words_4_letter_no_swears_file_name, 'w') as file:
    for word in words_4_no_swears:
        file.write(f"{word}\n")
with open(words_5_letter_no_swears_file_name, 'w') as file:
    for word in words_5_no_swears:
        file.write(f"{word}\n")
with open(words_6_letter_no_swears_file_name, 'w') as file:
    for word in words_6_no_swears:
        file.write(f"{word}\n")
with open(words_7_letter_no_swears_file_name, 'w') as file:
    for word in words_7_no_swears:
        file.write(f"{word}\n")
with open(words_8_letter_no_swears_file_name, 'w') as file:
    for word in words_8_no_swears:
        file.write(f"{word}\n")
with open(words_9_letter_no_swears_file_name, 'w') as file:
    for word in words_9_no_swears:
        file.write(f"{word}\n")
with open(words_10_letter_no_swears_file_name, 'w') as file:
    for word in words_10_no_swears:
        file.write(f"{word}\n")
with open(words_11_letter_no_swears_file_name, 'w') as file:
    for word in words_11_no_swears:
        file.write(f"{word}\n")
with open(words_12_letter_no_swears_file_name, 'w') as file:
    for word in words_12_no_swears:
        file.write(f"{word}\n")
with open(words_13_letter_no_swears_file_name, 'w') as file:
    for word in words_13_no_swears:
        file.write(f"{word}\n")
with open(words_14_letter_no_swears_file_name, 'w') as file:
    for word in words_14_no_swears:
        file.write(f"{word}\n")
with open(words_15_letter_no_swears_file_name, 'w') as file:
    for word in words_15_no_swears:
        file.write(f"{word}\n")             