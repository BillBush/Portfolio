This is a tool I wrote to extract data from vcf (variant call format) files.
Example arguments and call syntax are given above the Main method.  
Most of this code is specific to my project and its databases.  The one
exception is VcfDatalineToken.java.  It simply defines a grammar for parsing
the data section of a vcf file.  If you need to parse vcf data and you don't
have a parser, then this class is a great starting point.