import csv
tags = ['fluff', 'personal', 'shoutout', 'news', 'artwork', 'discussion', 'bug', 'highlight', 'complaint', 'guides & tips', 'suggestion', 'question', 'workshop', 'article']


# generate a combined corpus for MetApy
def generate_full_corpus():
     with open("../data/full_data.dat", "w") as ofile:
         with open("full_data/full_data.dat.labels", "w") as labels:
            for tag in tags:
                with open("data/dota2_data[" + tag.replace(" ", "") + "].csv", "r") as ifile:
                    reader = csv.reader(ifile, delimiter='|', quotechar='~')
                    fullwriter = csv.writer(ofile, delimiter=" ")
                    labelwriter = csv.writer(labels, delimiter=" ")
                    for row in reader:
                        fullwriter.writerow([row[0].replace("\n", "")+" "+row[2].replace("\n", "")+" "+row[1]])
                        labelwriter.writerow([tag])

# generate a combined file for use with Azure Machine Learning Services
def generate_full_multiple_column_domain_csv():
    with open("../data/full_data_multiple_column_domain.csv", "w") as ofile:
        for tag in tags:
            with open("data/dota2_data[" + tag.replace(" ", "") + "]_domain.csv", "r") as ifile:
                reader = csv.reader(ifile, delimiter='|', quotechar='~')
                fullwriter = csv.writer(ofile, delimiter=",")
                for row in reader:
                    fullwriter.writerow([row[0].replace("\n", "").replace(",", ""), row[1], row[2].replace("\n", " ").replace(",", ""), tag]) # title, url, text, tag

                        
generate_full_multiple_column_domain_csv()
