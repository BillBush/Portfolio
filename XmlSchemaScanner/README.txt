This tool generates relational table definitions, foreign key definitions,
and drop commands(all in ANSI SQL) from a specified xml file.  It can also
load data from said xml file into the generated database.  When I wrote this
the Java libraries for handling XML would attempt to load entire files into
memory.  This tool parses the file one line at a time from disk.  I use this
program all the time.  It's not pretty, but it gets the job done.