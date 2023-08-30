# This Python file contains all the methods which have 
# been used to compare the values among the answers of
# the members of the three groups

# They are based on the functions developed for the
# exploratory analysis

import matplotlib.pyplot as plt
import pandas as pd
import collections

import nltk
nltk.download('stopwords')
from nltk.corpus import stopwords
from nltk.stem import WordNetLemmatizer

from wordcloud import WordCloud

# ====================================
# ======== DATA RETRIEVAL ============
# ====================================
# These methods access the csv file to
# convert data in specific data struc-
# tures for visualization
# ====================================
# ====================================

# This method converts texts into frequency dictionary
def topwords_calculator(df_col): #
    text = " ".join(cat for cat in df_col).replace('\n', '') 

    wordcount = {} # Instantiate a dictionary

    # Frequency calculator

    for word in text.lower().split():
        #Preprocessing
        for char in [".", ",", ":", "\"", "!","\'s"]:                      #punctuation removal
            word = word.replace(char,"")         
        if word not in stopwords and word not in ["could", "without"]:     #stopwords removal
            word = lemmatizer.lemmatize(word)                              #lemmatization
            word = word.replace("colour","color")                          #AE / BE rectification
            # Start counter
            if word not in wordcount:
                wordcount[word] = 1
            else:
                wordcount[word] += 1
    return wordcount

# This method converts dict in top-words (set threshold
# of frequency: values below this limit are ignored)
def return_topwords(freq_dict, threshold):
    word_counter = collections.Counter(freq_dict)
    for word, count in word_counter.most_common():
        if count >= threshold:
            print(word, ": ", count)

# This method retrieves Plutchik emotions values and 
# stores them as dictionary
def emotion_data(dataDf, question):
    anger = dataDf['{0}_anger'.format(question)].astype(float).mean()
    disgust = dataDf['{0}_disgust'.format(question)].astype(float).mean()
    fear = dataDf['{0}_fear'.format(question)].astype(float).mean()
    interest = dataDf['{0}_interest'.format(question)].astype(float).mean()
    joy = dataDf['{0}_joy'.format(question)].astype(float).mean()
    sadness = dataDf['{0}_sadness'.format(question)].astype(float).mean()
    surprise = dataDf['{0}_surprise'.format(question)].astype(float).mean()
    trust = dataDf['{0}_trust'.format(question)].astype(float).mean()

    feelings = {
        "anger": round(anger, 2), 
        "disgust": round(disgust, 2), 
        "fear": round(fear, 2), 
        "anticipation": round(interest, 2), 
        "joy": round(joy, 2),
        "sadness": round(sadness, 2),
        "surprise": round(surprise, 2),
        "trust": round(trust, 2)}
    
    return feelings

# This method removes those answers which declared
# not to have recieved any benefit from this UX
def no_benefit(dataDf):
    no_occurrences = dataDf['q6'].str.contains('no', case=False, regex=False).sum() + dataDf['q6'].str.contains(r"(?i)don't").sum()
    print("Approximative amount of participants who denied any benefit: ", no_occurrences)

# ====================================
# ======== DATA VIZ ==================
# ====================================
# These methods take the functions of
# the previous section as an input and
# return different visualizations
# ====================================
# ====================================

# This method reads a word-frequency dict and
# provide a visualization as wordcloud
def wordcloud_generator(wc_dict):
    wordcloud = WordCloud(width = 1000, height = 500, background_color="white").generate_from_frequencies(wc_dict)

    plt.figure(figsize=(15,8))
    plt.imshow(wordcloud)
    plt.axis("off")

# This method plots as bars the values of the 
# five meaningfulness factors of Question 2
def meaningfulness_factor_plot(dataDf):
    paintings = dataDf['q2_music_dance'].astype(float).mean()
    war = dataDf['q2_war'].astype(float).mean()
    color_change = dataDf['q2_color'].astype(float).mean()
    conservation_data = dataDf['q2_conservation'].astype(float).mean()
    influence_cubism = dataDf['q2_cubism'].astype(float).mean()

    # Plot the barchart
    meaning_factors = {"Paintings": paintings, "War": war, "Color": color_change, "Conservation": conservation_data, "Avant-guardes": influence_cubism}

    keys = list(meaning_factors.keys())
    values = list(meaning_factors.values())
    positions = range(len(keys))

    plt.figure(figsize=(8, 6))
    bars = plt.bar(positions, values, tick_label=keys)

    plt.xlabel('Factor')
    plt.ylabel('Value')
    plt.title('Which factors were percieved as more meaningful during the experience?')

    for bar in bars:
        height = bar.get_height()
        plt.text(bar.get_x() + bar.get_width() / 2, height,
                '{:.2f}'.format(height), ha='center', va='bottom')
    plt.xticks(rotation = 45)

    plt.tight_layout()
    plt.show()

# This method plots piecharts and can be associated
# to multiple questions 
def pie_viz(dataDf, question_n, title):
    pie_df = dataDf[question_n].value_counts()
    print(pie_df)

    # Plotting a pie chart
    plt.figure(figsize=(8, 8))
    pie_df.plot(kind='pie', autopct='%1.1f%%')
    plt.axis('equal')
    plt.ylabel('')
    plt.title(title, pad=20)
    plt.legend()    
    plt.show()

# These last two methods provide a first visualization
# of the questions based on Plutchik. See the notebook
# "plutchick_emotions" for improved visualizations

# This method plots the value of each feeling
# in a barplot
def emotion_plot(dict_feel, title_plot):
    keys = list(dict_feel.keys())
    values = list(dict_feel.values())
    positions = range(len(keys))

    plt.figure(figsize=(8, 6))
    bars = plt.bar(positions, values, tick_label=keys)

    plt.xlabel('Feelings')
    plt.ylabel('Value')
    plt.title(title_plot)

    for bar in bars:
        height = bar.get_height()
        plt.text(bar.get_x() + bar.get_width() / 2, height,
                '{:.2f}'.format(height), ha='center', va='bottom')
    plt.xticks(rotation = 45)

    plt.tight_layout()
    plt.show()

# This method compares the value of the assessment
# task among the different artwork
def emotion_barcomparison(dataDf):
    keys = list(emotion_data(dataDf, "q4").keys())
    values1 = list(emotion_data(dataDf, "q4").values())
    values2 = list(emotion_data(dataDf, "q10").values())
    values3 = list(emotion_data(dataDf, "q12").values())

    x = range(len(keys))

    width = 0.2
    plt.figure(figsize=(10, 6))

    plt.bar(x, values1, width=width, label='Original')
    plt.bar([i + width for i in x], values2, width=width, label='Edit 1')
    plt.bar([i + 2 * width for i in x], values3, width=width, label='Edit 2')
    plt.xticks([i + width for i in x], keys)
    plt.ylabel('Values')

    plt.legend()

    return pd.DataFrame(
        {'Original': emotion_data(dataDf, "q4"), 
        'Edit 1': emotion_data(dataDf, "q10"), 
        'Edit 2': emotion_data(dataDf,"q12")})

# Prepare NLTK tools for textual pre-processing (valid for the entire dataset)
stopwords = stopwords.words('english')
lemmatizer = WordNetLemmatizer() 