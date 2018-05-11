import praw
import csv
import time
from prawcore.exceptions import ServerError

reddit = praw.Reddit(client_id = 'T0x6hrvUmWVRgQ', client_secret = 'qGNHgpq54sQmHsS1yHAdPe0iVXo', username = 'NaClino', password = 'Santin00', user_agent = 'praw_tutorial_1')

tags = ['fluff', 'personal', 'shoutout', 'news', 'artwork', 'announcement', 'discussion', 'bug', 'highlight', 'complaint', 'guides & tips', 'suggestion', 'question', 'esports', 'workshop', 'article']



time.sleep(30)
for tag in tags:
    output_file = "../data/individual_tags/dota2_data[" +tag.replace(" ", "") + "]_domain.csv"
    ofile = open(output_file, 'w')

    subreddit = reddit.subreddit('Dota2')
    write = csv.writer(ofile, delimiter='|', quotechar='~', quoting=csv.QUOTE_MINIMAL)


    search = subreddit.search('flair:"' + tag + '"', sort='top', limit=None)
    for post in search:
        if not post.link_flair_text is None:
            write.writerow([post.title.encode('ascii', errors='ignore'), post.domain.encode('ascii', errors='ignore'), post.selftext.encode('ascii', errors='ignore'), post.link_flair_css_class]) # [title, link, text, tag]
    print ("sleeping after " + tag)
    time.sleep(30)
