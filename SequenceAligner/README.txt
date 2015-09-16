This is the Visual Studio 2013 solution for an application demonstrating basic sequence alignment algorithms. The actual executable can be found in Portfolio/SequenceAligner/SequenceAligner/bin/Release. The program computes global, local, and longest common substring alignments for a given pair of strings.  For global and local alignments a substitution matrix specifying match/mismatch penalties must be provided along with an indel penalty specified either as a fixed integer or an affine gap function.  Example input sequences and parameters are provided.  In addition to aligned sequences the program also displays the corresponding dynamic programming table along with backtrack paths for each alignment generated.

This application is intended to be a lecture tool.  As such it has certain limitations that prevent it from be useful in a laboratory setting.
1) In all cases numeric parameters should be limited to integers.  
2) One should not specify an affine gap function where a fixed indel penalty will do.  
3) Only linear affine gap functions are supported.
4) The algorithms and coding practices applied are not particularly efficient.  This is in part to allow room for "optimization" homeowrk assignments.  It is also intended to unify certain concepts.  For instance the algorithm for computing longest common substring was chosen primarily for its reuse of code and concepts from the other alignment algorithms.  It is not the best way to calculate longest common substring as one does not require dynamic programing to do this.
5) The user should limit his/her input to strings no greater than 20 characters in length.  This is partially due to choice of alignment algorithms but also because all alignments for a given pair of strings will be generated.

Please include the following with any bugs reported:
1) Input file
2) Parameter file
3) Output file
4) Screenshot with bug values circled in red.
5) A written explanation of what should be displayed.
6) Numbered steps for reproducing the bug detailing EVERY user input from application start to bug manifestation.

