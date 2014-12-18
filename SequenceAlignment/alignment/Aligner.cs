using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aligner
{
    class AlignerException : Exception { protected internal AlignerException(String message) : base(message) { } }
    /// <summary>
    /// This class represents a primary unit or 'letter' in our Alphabet.
    /// The Alphabet class defines textual mappings for each Symbol.
    /// This class merely specifies a mapping of penalties for each symbol
    /// that apply when it is replaced by another.  This design was chosen
    /// so as to allow symbol replace to not be associative, even though it
    /// usually is.
    /// </summary>
    class Symbol
    {
        private readonly Dictionary<Symbol, int> penalties = new Dictionary<Symbol, int>();
        protected internal int this[Symbol key] { get { int i; penalties.TryGetValue(key, out i); return i; } set { penalties.Add(key, value); } }
    }
    /// <summary>
    /// This class maps a set of Strings to Symbol objects each with its
    /// own replacement rules.  Strings were chosen for our mappings
    /// instead of chars because, even though char's would be more
    /// effecient, Strings are less restrictive.
    /// </summary>
    class Alphabet
    {
        private readonly Dictionary<String, Symbol> symbols = new Dictionary<string, Symbol>();
        private int indelPenalty = int.MaxValue;
        private int h = int.MaxValue;
        private int s = int.MaxValue;
        /// <param name="input">The entire contents of a valid parameter file</param>
        /// <remarks>TODO: Add robust syntax checking to help user find and correct parameter format errors</remarks>
        protected internal Alphabet(String input)
        {
            String[] symbolAry = null; int row = 0;
            while ((input = input.Trim()).Length > 0)//process parameters one line at a time
            {
                //take 1 line out of input string
                String line = input.Substring(0, Regex.Match(input, "^.*$", RegexOptions.Multiline | RegexOptions.CultureInvariant).Length);
                input = input.Substring(line.Length);
                //remove comments
                if (line.Contains(";")) { line = line.Substring(0, line.IndexOf(";")); }
                line = line.Trim();
                //skip blank or comment-only lines
                if (line.Equals("")) { continue; }
                //remove consecutive spaces;  This actually replaces all "one or more" with one space
                for (Match m = Regex.Match(line, "[ ][ ][ ]*"); m.Success; m = Regex.Match(line, "[ ][ ][ ]*")) { line = line.Replace(m.ToString(), " "); }
                //load alphabet
                if (symbolAry == null) { foreach (String s in (symbolAry = line.Split(' '))) { symbols.Add(s.ToUpper(), new Symbol()); } }
                //load aligned penalties
                else if (row < symbolAry.Length)
                {
                    String[] rowValues = line.Split(' ');
                    for (int col = 0; col < symbolAry.Length; col++) { this[symbolAry[col]][this[symbolAry[row]]] = int.Parse(rowValues[col]); }
                    row++;
                }
                //load indel or affine penalty
                else if (indelPenalty == int.MaxValue)
                {
                    try { indelPenalty = int.Parse(line); }
                    catch (FormatException fe)
                    {
                        //Affien gap function regular expression
                        //"g[ ]*\\([ ]*q[ ]*\\)[ ]*=[ ]*(-|)[ ]*[0-9][0-9]*[ ]*[-+][ ]*[0-9][0-9]*[ ]*q"
                        line = line.Substring(Regex.Match(line, "g[ ]*\\([ ]*q[ ]*\\)[ ]*=[ ]*").Length).Trim();
                        Match m = Regex.Match(line, "(-|)[ ]*[0-9][0-9]*[ ]*");
                        line = line.Substring(m.Length).Trim();
                        h = int.Parse(m.Value.Replace(" ", "").Trim());
                        s = int.Parse(Regex.Match(line, "[-+][ ]*[0-9][0-9]*[ ]*").Value.Replace(" ", "").Trim());
                    }
                }
            }
        }
        protected internal Symbol this[String key] { get { Symbol s; symbols.TryGetValue(key.ToUpper(), out s); return s; } }
        protected internal int IndelPenalty { get { return indelPenalty; } }
        protected internal int this[String from, String to] { get { return this[from.ToUpper()][this[to.ToUpper()]]; } }
        protected internal int AffinePenalty(int gapCount) { return -h - s * gapCount; }
        protected internal int S { get { return s; } }
        protected internal int H { get { return h; } }
    }
    /// <summary>
    /// This class is just a wrapper for a string containing an entire
    /// sequence.  It is basically a just a String with a name field.
    /// It also overrides and overloads ToString to provide additional
    /// formatting
    /// </summary>
    class Sequence
    {
        private readonly String name;
        private String value;

        protected internal Sequence(String name) { this.name = name.Trim(); }
        protected internal Sequence(String name, String value) : this(name) { this.value = value.Trim(); }
        protected internal Sequence(Sequence seqToCopy) { this.name = String.Copy(seqToCopy.name).Trim(); this.value = String.Copy(seqToCopy.value).Trim(); }
        protected internal String Value { get { return value; } set { this.value = value.Trim(); } }
        protected internal String Name { get { return name; } }
        /// <summary>
        /// Builds a List of Sequences from the String representation of
        /// an input file.
        /// </summary>
        /// <returns>A List of Sequence objects</Sequence></returns>
        /// <param name="input">The entire contents of a valid input file</param>
        /// <remarks>TODO: Add robust syntax checking to help user find and correct sequence format errors</remarks>
        static protected internal List<Sequence> extractSequences(String input)
        {
            List<Sequence> sequences = new List<Sequence>();
            Sequence seq = null;
            //process input file one line at a time
            while ((input = input.Trim()).Length > 0)
            {
                //take 1 line out of input string
                String line = input.Substring(0, Regex.Match(input, "^.*$", RegexOptions.Multiline | RegexOptions.CultureInvariant).Length);
                input = input.Substring(line.Length);
                //remove comments
                if (line.Contains(";")) { line = line.Substring(0, line.IndexOf(";")); }
                line = line.Trim();
                //skip blank or comment-only lines
                if (line.Equals("")) { continue; }
                //check for new sequence start, and create new Sequence if found
                if (line.StartsWith(">"))
                {
                    seq = new Sequence(line);
                    sequences.Add(seq);
                    continue;
                }
                else { seq.Value += line.Replace(" ", ""); }//append to Sequence value after removing any spaces.
            }
            return sequences;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name);
            Aligner.addLineSeperator(sb);
            sb.Append(value);
            return sb.ToString();
        }
        public String ToString(int maxCharsPerLine)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Aligner.makeMultiLine(maxCharsPerLine, name));
            Aligner.addLineSeperator(sb);
            sb.Append(Aligner.makeMultiLine(maxCharsPerLine, Value));
            return sb.ToString();
        }
    }
    /// <summary>
    /// This class stores the edits allowed at any given step of our
    /// dynamic programming algorithm.  A struct was used to reduce heap
    /// usage and simplify code.  To avoid future restrictions the class
    /// was designed so as to allow Match and Mismatch alignments to be
    /// treated differently.  Operators were overloaded to slim down code
    /// and hopefully make it more readable.
    /// </summary>
    struct StringEdit
    {
        [Flags]
        public enum EditType { None = 0, Match = 1, Mismatch = 2, Insert = 4, Delete = 8, Skip = 16 }

        private int value;
        private EditType edits;

        public StringEdit(int value, EditType edits) { this.value = value; this.edits = edits; }
        public StringEdit(StringEdit se, EditType edits) { this.value = se.value; this.edits = edits; }
        public int Value { get { return value; } set { this.value = value; } }
        public EditType Edits { get { return edits; } set { edits = value; } }
        public bool IsEmpty { get { return this.edits == EditType.None; } }
        static public bool operator <(StringEdit lhs, StringEdit rhs) { return lhs.value < rhs.value; }
        static public bool operator >(StringEdit lhs, StringEdit rhs) { return lhs.value > rhs.value; }
        static public bool operator ==(StringEdit lhs, StringEdit rhs) { return lhs.value == rhs.value; }
        static public bool operator !=(StringEdit lhs, StringEdit rhs) { return lhs.value != rhs.value; }
        static public StringEdit operator -(StringEdit lhs, int rhs) { if (lhs.value != int.MinValue) { lhs.value -= rhs; } return lhs; }
        static public StringEdit operator +(StringEdit lhs, int rhs) { if (lhs.value != int.MaxValue) { lhs.value += rhs; } return lhs; }
        static public StringEdit operator |(StringEdit lhs, StringEdit rhs)
        {
            if (lhs < rhs) { return rhs; }
            else if (lhs > rhs) { return lhs; }
            else if (lhs == rhs) { return new StringEdit(rhs.Value, rhs.Edits | lhs.Edits); }
            else { throw new AlignerException("Invalid state reached!"); }
        }
        public StringEdit delete(int penalty) { return new StringEdit((value != int.MinValue && value != int.MaxValue) ? (value + penalty) : value, EditType.Delete); }
        public StringEdit insert(int penalty) { return new StringEdit((value != int.MinValue && value != int.MaxValue) ? (value + penalty) : value, EditType.Insert); }
        public StringEdit align(int penalty) { return new StringEdit((value != int.MinValue && value != int.MaxValue) ? (value + penalty) : value, (penalty < 0) ? EditType.Mismatch : EditType.Match); }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(value);
            sb.Append(" ");
            for (int i = 1; i < 16; i *= 2)
            {
                if (edits.HasFlag((EditType)i))
                {
                    sb.Append((EditType)i);
                    sb.Append(" ");
                }
            }
            return base.ToString();
        }
    }
    /// <summary>
    /// This class holds an alingment between two complete Sequences
    /// </summary>
    class PairWiseAlignment
    {
        protected internal Sequence top;
        protected internal Sequence left;
        protected internal PairWiseAlignment(Sequence top, Sequence left) { this.top = new Sequence(top); this.left = new Sequence(left); }
        protected internal PairWiseAlignment(PairWiseAlignment alignment) : this(alignment.top, alignment.left) { }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(top.ToString());
            Aligner.addLineSeperator(sb);
            sb.Append(left.ToString());
            return sb.ToString();
        }
        public String ToString(int maxCharsPerLine) { return top.ToString(maxCharsPerLine) + "\r\n\r\n" + left.ToString(maxCharsPerLine); }
    }
    /// <summary>
    /// This class represents the table calculated in a dynamic
    /// programming alignment problem.
    /// </summary>
    abstract class AlignmentTable
    {
        protected Sequence top;
        protected Sequence left;
        protected StringEdit[,] dpTable;
        protected Alphabet alphabet;
        protected List<PairWiseAlignment> alignments;
        protected int maxAlignmentCount;
        protected int maxScore = int.MinValue;

        protected internal AlignmentTable(Sequence top, Sequence left, Alphabet alphabet)
        {
            this.top = new Sequence(top);
            this.left = new Sequence(left);
            this.alphabet = alphabet;
            this.alignments = new List<PairWiseAlignment>();
            this.maxAlignmentCount = 1;
        }
        protected internal Sequence Left { get { return left; } }
        protected internal Sequence Top { get { return top; } }
        protected internal int MaxScore { get { return maxScore; } }
        /// <summary>
        /// Specifies the maximum number of alignments to be returned by GenAlignments
        /// </summary>
        protected internal int MaxAlignmentCount { get { return maxAlignmentCount; } set { maxAlignmentCount = value; } }
        /// <summary>
        /// Populates the dynamic programming table based on the rules of the implimenting alignment
        /// algorithm
        /// </summary>
        abstract protected void GenTable();
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>The StringEdit, i.e. maximum value and associated changes allowed, at a specific
        /// point in the dynamic programming table based on the rules of the implimenting alignment</returns>
        abstract protected StringEdit CalcEdit(int row, int col);
        /// <summary>
        /// Generates alignments from the populated dynamic programming table based on the rules of the
        /// implimenting alignment algorithm
        /// </summary>
        /// <returns>A List of PairWiseAlignments</returns>
        abstract protected internal List<PairWiseAlignment> GenAlignments();
    }
    /// <summary>
    /// This class extends AlignmentTable with initialization
    /// rules and an implementation of cost function
    /// </summary>
    class GlobalAlignmentTable : AlignmentTable
    {
        protected internal GlobalAlignmentTable(Sequence top, Sequence left, Alphabet alphabet) : base(top, left, alphabet) { }
        /// <summary>
        /// Populates the dynamic programming table
        /// </summary>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        override protected void GenTable()
        {
            if (dpTable == null)//create table and initialize top row and left column if we haven't already.
            {
                dpTable = new StringEdit[this.left.Value.Length + 1, this.top.Value.Length + 1];
                dpTable[0, 0] = new StringEdit(0, StringEdit.EditType.Match | StringEdit.EditType.Mismatch);
                for (int i = 1; i < top.Value.Length + 1; i++) { dpTable[0, i] = new StringEdit(alphabet.IndelPenalty * i, StringEdit.EditType.Delete); }
                for (int i = 1; i < left.Value.Length + 1; i++) { dpTable[i, 0] = new StringEdit(alphabet.IndelPenalty * i, StringEdit.EditType.Insert); }
            }
            //re-initialize table if it wasn't just created
            else { for (int row = 1; row < left.Value.Length + 1; row++) { for (int col = 1; col < top.Value.Length + 1; col++) { dpTable[row, col] = new StringEdit(); } } }
            //populate the table
            for (int row = 1; row < left.Value.Length + 1; row++) { for (int col = 1; col < top.Value.Length + 1; col++) { CalcEdit(row, col); } }
        }
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>The StringEdit, i.e. maximum value and associated changes allowed, at a specific
        /// point in the dynamic programming table</returns>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        override protected StringEdit CalcEdit(int row, int col)
        {
            //exit conditions: the value has already been calculated or has been defaulted
            if ((!dpTable[row, col].IsEmpty) || dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Skip)) { return dpTable[row, col]; }
            //Apply cost functions
            dpTable[row, col] = CalcEdit(row - 1, col).insert(alphabet.IndelPenalty)
                | CalcEdit(row, col - 1).delete(alphabet.IndelPenalty)
                 | CalcEdit(row - 1, col - 1).align(alphabet[left.Value[row - 1].ToString(), top.Value[col - 1].ToString()]);

            return dpTable[row, col];
        }
        /// <summary>
        /// Generates alignments from the populated dynamic programming table
        /// </summary>
        /// <returns>A List of PairWiseAlignments</returns>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        override protected internal List<PairWiseAlignment> GenAlignments()
        {
            GenTable();
            alignments.Clear();
            maxScore = dpTable[left.Value.Length, top.Value.Length].Value;
            GenAlignments(left.Value.Length, top.Value.Length, new PairWiseAlignment(top, left));
            return alignments;
        }
        /// <summary>
        /// Generates alignments from a specific point in the populated dynamic programming table
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="alignmentSoFar"></param>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        virtual protected void GenAlignments(int row, int col, PairWiseAlignment alignmentSoFar)
        {
            if (alignments.Count == MaxAlignmentCount) { return; }//exit conditions: the maximum number of alignments have been generated
            if (row == 0 && col == 0) { alignments.Add(alignmentSoFar); return; }//exit condition: we have finnished generating the alignment
            if (dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Delete))//recursively branch on delete
            {
                PairWiseAlignment deleteAlignment = new PairWiseAlignment(alignmentSoFar);
                deleteAlignment.left.Value = deleteAlignment.left.Value.Insert(row, " - ");
                GenAlignments(row, col - 1, deleteAlignment);
            }
            if (dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Insert))//recursively branch on insert
            {
                PairWiseAlignment insertAlignment = new PairWiseAlignment(alignmentSoFar);
                insertAlignment.top.Value = insertAlignment.top.Value.Insert(col, " - ");
                GenAlignments(row - 1, col, insertAlignment);
            }
            //recursively continue on match or mismatch
            if (dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Match) || dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Mismatch))
            {
                GenAlignments(row - 1, col - 1, alignmentSoFar);
            }
        }
    }
    /// <summary>
    /// This class extends GlobalAlignmentTable with initialization
    /// rules and an implementation of cost function
    /// </summary>
    class LocalAlignmentTable : GlobalAlignmentTable
    {
        protected List<Point> maxEditPoints;
        protected internal LocalAlignmentTable(Sequence top, Sequence left, Alphabet alphabet) : base(top, left, alphabet) { maxEditPoints = new List<Point>(); }
        /// <summary>
        /// Populates the dynamic programming table
        /// </summary>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        override protected void GenTable()
        {
            if (dpTable == null)//create table and initialize top row and left column if we haven't already.
            {
                dpTable = new StringEdit[this.left.Value.Length + 1, this.top.Value.Length + 1];
                dpTable[0, 0] = new StringEdit(0, StringEdit.EditType.Match | StringEdit.EditType.Mismatch);
                for (int i = 1; i < top.Value.Length + 1; i++) { dpTable[0, i] = new StringEdit(0, StringEdit.EditType.Skip); }
                for (int i = 1; i < left.Value.Length + 1; i++) { dpTable[i, 0] = new StringEdit(0, StringEdit.EditType.Skip); }
            }
            //re-initialize table if it wasn't just created
            else { for (int row = 1; row < left.Value.Length + 1; row++) { for (int col = 1; col < top.Value.Length + 1; col++) { dpTable[row, col] = new StringEdit(); } } }
            //populate the table
            for (int row = 1; row < left.Value.Length + 1; row++)
            {
                for (int col = 1; col < top.Value.Length + 1; col++)
                {
                    int score = CalcEdit(row, col).Value;
                    if (maxScore > score) { continue; }
                    if (maxScore < score) { maxEditPoints.Clear(); maxScore = score; }
                    maxEditPoints.Add(new Point(row, col));
                }
            }
        }
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>The StringEdit, i.e. maximum value and associated changes allowed, at a specific
        /// point in the dynamic programming table</returns>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        override protected StringEdit CalcEdit(int row, int col) { return dpTable[row, col] = base.CalcEdit(row, col) | new StringEdit(0, StringEdit.EditType.Skip); }
        /// <summary>
        /// Generates alignments from the populated dynamic programming table
        /// </summary>
        /// <returns>A List of PairWiseAlignments</returns>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        override protected internal List<PairWiseAlignment> GenAlignments()
        {
            GenTable();
            alignments.Clear();
            foreach (Point p in maxEditPoints)
            {
                GenAlignments(p.X, p.Y, new PairWiseAlignment(new Sequence(top.Name, top.Value.Substring(0, p.Y)), new Sequence(left.Name, left.Value.Substring(0, p.X))));
            }
            return alignments;
        }
        /// <summary>
        /// Generates alignments from a specific point in the populated dynamic programming table
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="alignmentSoFar"></param>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        override protected void GenAlignments(int row, int col, PairWiseAlignment alignmentSoFar)
        {
            if (alignments.Count == MaxAlignmentCount) { return; }//exit condition: the maximum number of alignments have been generated
            if (dpTable[row, col].Value == 0)//exit condition: we have finnished the alignment
            {
                alignmentSoFar.left.Value = alignmentSoFar.left.Value.Substring(row);
                alignmentSoFar.top.Value = alignmentSoFar.top.Value.Substring(col);
                alignments.Add(alignmentSoFar);
                return;
            }
            if (dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Delete))//recursively branch on Delete
            {
                PairWiseAlignment deleteAlignment = new PairWiseAlignment(alignmentSoFar);
                deleteAlignment.left.Value = deleteAlignment.left.Value.Insert(row, " - ");
                GenAlignments(row, col - 1, deleteAlignment);
            }
            if (dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Insert))//recursively branch on Insert
            {
                PairWiseAlignment insertAlignment = new PairWiseAlignment(alignmentSoFar);
                insertAlignment.top.Value = insertAlignment.top.Value.Insert(col, " - ");
                GenAlignments(row - 1, col, insertAlignment);
            }
            //recursively continue on Match or Mismatch
            if (dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Match) || dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Mismatch))
            {
                GenAlignments(row - 1, col - 1, alignmentSoFar);
            }
        }
    }
    /// <summary>
    /// This class extends AlignmentTable with initialization
    /// rules and an implementation of a cost function
    /// enforcing afine gap extensions
    /// </summary>
    class AffineGlobalAlignmentTable : AlignmentTable
    {
        protected StringEdit[,] dpTableG;//alignment score table
        protected StringEdit[,] dpTableE;//delete score table
        protected StringEdit[,] dpTableF;//insert score table

        protected internal AffineGlobalAlignmentTable(Sequence top, Sequence left, Alphabet alphabet) : base(top, left, alphabet) { }
        /// <summary>
        /// Calculates the penalty associated with alignment and stores it in the alignment score table
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        protected virtual StringEdit getG(int row, int col)
        {
            if ((!dpTableG[row, col].IsEmpty) || dpTableG[row, col].Edits.HasFlag(StringEdit.EditType.Skip)) { return dpTableG[row, col]; }
            StringEdit ret;
            if (row == 0 || col == 0)
            {
                ret = new StringEdit(int.MinValue, StringEdit.EditType.Mismatch | StringEdit.EditType.Match);
            }
            else
            {
                ret = CalcEdit(row - 1, col - 1) + alphabet[left.Value[row - 1].ToString(), top.Value[col - 1].ToString()];
            }
            return dpTableG[row, col] = ret;
        }
        /// <summary>
        /// Calculates the penalty associated with an insert and stores it in the insert score table
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        protected virtual StringEdit getF(int row, int col)
        {
            if ((!dpTableF[row, col].IsEmpty) || dpTableF[row, col].Edits.HasFlag(StringEdit.EditType.Skip)) { return dpTableF[row, col]; }
            StringEdit ret;
            if (row == 0)
            {
                if (col == 0)
                {
                    ret = new StringEdit(0, StringEdit.EditType.Skip);
                }
                else
                {
                    ret = new StringEdit(int.MinValue, StringEdit.EditType.Insert);
                }
            }
            else
            {
                if (col == 0)
                {
                    ret = new StringEdit(alphabet.AffinePenalty(row), StringEdit.EditType.Insert);
                }
                else
                {
                    ret = new StringEdit(getF(row - 1, col) - alphabet.S, StringEdit.EditType.Insert) | new StringEdit(CalcEdit(row - 1, col) - alphabet.H - alphabet.S, StringEdit.EditType.Match | StringEdit.EditType.Mismatch);
                }
            }
            return dpTableF[row, col] = ret;
        }
        /// <summary>
        /// Calculates the penalty associated with a delete and stores it in the delete penalty table
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        protected virtual StringEdit getE(int row, int col)
        {
            if ((!dpTableE[row, col].IsEmpty) || dpTableE[row, col].Edits.HasFlag(StringEdit.EditType.Skip)) { return dpTableE[row, col]; }
            StringEdit ret;
            if (row == 0)
            {
                if (col == 0)
                {
                    ret = new StringEdit(0, StringEdit.EditType.Skip);
                }
                else
                {
                    ret = new StringEdit(alphabet.AffinePenalty(col), StringEdit.EditType.Delete);
                }
            }
            else
            {
                if (col == 0)
                {
                    ret = new StringEdit(int.MinValue, StringEdit.EditType.Delete);
                }
                else
                {
                    ret = new StringEdit(getE(row, col - 1) - alphabet.S, StringEdit.EditType.Delete) | new StringEdit(CalcEdit(row, col - 1) - alphabet.H - alphabet.S, StringEdit.EditType.Match | StringEdit.EditType.Mismatch);
                }
            }
            return dpTableE[row, col] = ret;
        }
        /// <summary>
        /// Recursive cost function.  It keeps track of the highest scoring chain(s) of StringEdits
        /// on which we will backtrack to obtain maximum scoring alignments
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        override protected StringEdit CalcEdit(int row, int col)
        {
            if ((!dpTable[row, col].IsEmpty) || dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Skip)) { return dpTable[row, col]; }
            StringEdit ret;
            if (row == 0)
            {
                if (col == 0)
                {
                    ret = new StringEdit(0, StringEdit.EditType.Skip);
                }
                else
                {
                    ret = new StringEdit(getE(row, col), StringEdit.EditType.Delete);
                }
            }
            else
            {
                if (col == 0)
                {
                    ret = new StringEdit(getF(row, col), StringEdit.EditType.Insert);
                }
                else
                {
                    ret = new StringEdit(getG(row, col), StringEdit.EditType.Match | StringEdit.EditType.Mismatch)
                        | new StringEdit(getF(row, col), StringEdit.EditType.Insert)
                        | new StringEdit(getE(row, col), StringEdit.EditType.Delete);
                }
            }
            return dpTable[row, col] = ret;
        }
        /// <summary>
        /// Initialize and populate scoring tables, 
        /// </summary>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        override protected void GenTable()
        {
            dpTable = new StringEdit[this.left.Value.Length + 1, this.top.Value.Length + 1];
            dpTableG = new StringEdit[this.left.Value.Length + 1, this.top.Value.Length + 1];
            dpTableE = new StringEdit[this.left.Value.Length + 1, this.top.Value.Length + 1];
            dpTableF = new StringEdit[this.left.Value.Length + 1, this.top.Value.Length + 1];
            dpTable[0, 0] = dpTableE[0, 0] = dpTableF[0, 0] = dpTableG[0, 0] = new StringEdit(0, StringEdit.EditType.Match | StringEdit.EditType.Mismatch);
            for (int i = 1; i < left.Value.Length; i++) { dpTable[i, 0] = dpTableF[i, 0] = new StringEdit(alphabet.AffinePenalty(i), StringEdit.EditType.Insert); dpTableG[i, 0] = dpTableE[i, 0] = new StringEdit(int.MinValue, StringEdit.EditType.Skip); }
            for (int i = 1; i < top.Value.Length; i++) { dpTable[0, i] = dpTableE[0, i] = new StringEdit(alphabet.AffinePenalty(i), StringEdit.EditType.Delete); dpTableG[0, i] = dpTableF[0, i] = new StringEdit(int.MinValue, StringEdit.EditType.Skip); }
            for (int row = 1; row < left.Value.Length + 1; row++)
            {
                for (int col = 1; col < top.Value.Length + 1; col++)
                {
                    CalcEdit(row, col);
                }
            }
        }
        /// <summary>
        /// Generates allignments from the populated scoring tables
        /// </summary>
        /// <returns></returns>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        override protected internal List<PairWiseAlignment> GenAlignments()
        {
            GenTable();
            alignments.Clear();
            maxScore = dpTable[left.Value.Length, top.Value.Length].Value;
            int row = left.Value.Length;
            int col = top.Value.Length;
            PairWiseAlignment alignmentSoFar = new PairWiseAlignment(top, left);
            if (dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Delete))//recursively branch on delete
            {
                GenAlignments(row, col, alignmentSoFar, dpTableE);
            }
            if (dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Insert))//recursively branch on insert
            {
                GenAlignments(row, col, alignmentSoFar, dpTableF);
            }
            if (dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Match) || dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Mismatch))
            {
                GenAlignments(row, col, alignmentSoFar, dpTableG);
            }
            return alignments;
        }
        /// <summary>
        /// Generates alignments from a specific point in a specific scoring table
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="alignmentSoFar"></param>
        /// <param name="table"></param>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        protected virtual void GenAlignments(int row, int col, PairWiseAlignment alignmentSoFar, StringEdit[,] table)
        {
            if (alignments.Count == MaxAlignmentCount) { return; }//exit conditions: the maximum number of alignments have been generated
            if (row == 0 && col == 0) { alignments.Add(alignmentSoFar); return; }//exit condition: we have finnished generating the alignment
            PairWiseAlignment nextAlignment = null;
            StringEdit.EditType et = table[row, col].Edits;
            //perform string modification and select next coordinate based on the currently selected scoring table
            if (table == dpTableG)
            {
                nextAlignment = alignmentSoFar;
                row--;
                col--;
            }
            else
            {
                nextAlignment = new PairWiseAlignment(alignmentSoFar);
                if (table == dpTableE)
                {
                    nextAlignment.left.Value = nextAlignment.left.Value.Insert(row, " - ");
                    col--;
                }
                else if (table == dpTableF)
                {
                    nextAlignment.top.Value = nextAlignment.top.Value.Insert(col, " - ");
                    row--;
                }
            }
            //recursively branch based on the type of StringEdit preceding the current StringEdit
            //recursively branch on delete
            if (et.HasFlag(StringEdit.EditType.Delete)) { GenAlignments(row, col, nextAlignment, dpTableE); }
            //recursively branch on insert
            if (et.HasFlag(StringEdit.EditType.Insert)) { GenAlignments(row, col, nextAlignment, dpTableF); }
            //recursively continue on match or mismatch
            if (et.HasFlag(StringEdit.EditType.Match) || dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Mismatch)) { GenAlignments(row, col, nextAlignment, dpTableG); }
        }
    }
    /// <summary>
    /// This class extends AffineGlobalAlignmentTable to perform local
    /// alignments via overriden initialization and backtracking rules
    /// </summary>
    class AffineLocalAlignmentTable : AffineGlobalAlignmentTable
    {
        private List<Point> maxEditPoints;

        protected internal AffineLocalAlignmentTable(Sequence top, Sequence left, Alphabet alphabet) : base(top, left, alphabet) { maxEditPoints = new List<Point>(); }
        /// <summary>
        /// Initialize and populate scoring tables, 
        /// </summary>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        protected override void GenTable()
        {
            dpTable = new StringEdit[this.left.Value.Length + 1, this.top.Value.Length + 1];
            dpTableG = new StringEdit[this.left.Value.Length + 1, this.top.Value.Length + 1];
            dpTableE = new StringEdit[this.left.Value.Length + 1, this.top.Value.Length + 1];
            dpTableF = new StringEdit[this.left.Value.Length + 1, this.top.Value.Length + 1];
            dpTable[0, 0] = dpTableE[0, 0] = dpTableF[0, 0] = dpTableG[0, 0] = new StringEdit(0, StringEdit.EditType.Match | StringEdit.EditType.Mismatch);
            for (int i = 1; i < left.Value.Length + 1; i++)
            {
                dpTable[i, 0] = dpTableG[i, 0] = new StringEdit(0, StringEdit.EditType.Skip);
                dpTableF[i, 0] = dpTableE[i, 0] = new StringEdit(int.MinValue, StringEdit.EditType.Skip);
            }
            for (int i = 1; i < top.Value.Length + 1; i++)
            {
                dpTable[0, i] = dpTableG[0, i] = new StringEdit(0, StringEdit.EditType.Skip);
                dpTableE[0, i] = dpTableF[0, i] = new StringEdit(int.MinValue, StringEdit.EditType.Skip);
            }
            for (int row = 1; row < left.Value.Length + 1; row++)
            {
                for (int col = 1; col < top.Value.Length + 1; col++)
                {
                    int score = CalcEdit(row, col).Value;
                    if (maxScore > score) { continue; }
                    if (maxScore < score) { maxEditPoints.Clear(); maxScore = score; }
                    maxEditPoints.Add(new Point(row, col));
                }
            }
        }
        /// <summary>
        /// Recursive cost function.  It keeps track of the highest scoring chain(s) of StringEdits
        /// on which we will backtrack to obtain maximum scoring alignments
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        override protected StringEdit CalcEdit(int row, int col) { return dpTable[row, col] = base.CalcEdit(row, col) | new StringEdit(0, StringEdit.EditType.Skip); }
        /// <summary>
        /// Generates allignments from the populated scoring tables
        /// </summary>
        /// <returns></returns>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        protected internal override List<PairWiseAlignment> GenAlignments()
        {
            GenTable();
            alignments.Clear();
            foreach (Point p in maxEditPoints)
            {
                int row = p.X;
                int col = p.Y;
                PairWiseAlignment alignmentSoFar = new PairWiseAlignment(new Sequence(top.Name, top.Value.Substring(0, p.Y)), new Sequence(left.Name, left.Value.Substring(0, p.X)));
                if (dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Delete))//recursively branch on delete
                {
                    GenAlignments(row, col, alignmentSoFar, dpTableE);
                }
                if (dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Insert))//recursively branch on insert
                {
                    GenAlignments(row, col, alignmentSoFar, dpTableF);
                }
                if (dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Match) || dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Mismatch))
                {
                    GenAlignments(row, col, alignmentSoFar, dpTableG);
                }
            }
            return alignments;
        }
        /// <summary>
        /// Generates alignments from a specific point in a specific scoring table
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="alignmentSoFar"></param>
        /// <param name="table"></param>
        /// <remarks>WARNING: A recursive and potentially memmory intensive operation</remarks>
        protected override void GenAlignments(int row, int col, PairWiseAlignment alignmentSoFar, StringEdit[,] table)
        {
            if (alignments.Count == MaxAlignmentCount) { return; }//exit conditions: the maximum number of alignments have been generated
            if (row == 0 && col == 0) { alignments.Add(alignmentSoFar); return; }//exit condition: we have finnished generating the alignment
            PairWiseAlignment nextAlignment = null;
            StringEdit.EditType et = table[row, col].Edits;
            //perform string modification and select next coordinate based on the currently selected scoring table
            if (table == dpTableG)
            {
                nextAlignment = alignmentSoFar;
                row--;
                col--;
            }
            else
            {
                nextAlignment = new PairWiseAlignment(alignmentSoFar);
                if (table == dpTableE)
                {
                    nextAlignment.left.Value = nextAlignment.left.Value.Insert(row, " - ");
                    col--;
                }
                else if (table == dpTableF)
                {
                    nextAlignment.top.Value = nextAlignment.top.Value.Insert(col, " - ");
                    row--;
                }
            }
            if (table[row, col].Value == 0 || et.HasFlag(StringEdit.EditType.Skip))//exit condition: we have finnished the alignment
            {
                alignmentSoFar.left.Value = alignmentSoFar.left.Value.Substring(row);
                alignmentSoFar.top.Value = alignmentSoFar.top.Value.Substring(col);
                alignments.Add(alignmentSoFar);
                return;
            }
            //recursively branch based on the type of StringEdit preceding the current StringEdit
            //recursively branch on delete
            if (et.HasFlag(StringEdit.EditType.Delete)) { GenAlignments(row, col, nextAlignment, dpTableE); }
            //recursively branch on insert
            if (et.HasFlag(StringEdit.EditType.Insert)) { GenAlignments(row, col, nextAlignment, dpTableF); }
            //recursively continue on match or mismatch
            if (et.HasFlag(StringEdit.EditType.Match) || dpTable[row, col].Edits.HasFlag(StringEdit.EditType.Mismatch)) { GenAlignments(row, col, nextAlignment, dpTableG); }
        }
    }
    /// <summary>
    /// This class represents a list of Sequences and their Alphabet
    /// </summary>
    class Aligner
    {
        private Alphabet alphabet;
        private List<Sequence> sequences;

        protected internal Aligner() { }
        protected internal String Parameters { set { alphabet = new Alphabet(value); } }
        protected internal String Input { set { sequences = Sequence.extractSequences(value); } }
        protected internal String getGlobalAlignment()
        {
            List<PairWiseAlignment> alignments;
            AlignmentTable at;
            if (alphabet.IndelPenalty == int.MaxValue)
            {
                at = new AffineGlobalAlignmentTable(sequences[0], sequences[1], alphabet);
            }
            else
            {
                at = new GlobalAlignmentTable(sequences[0], sequences[1], alphabet);
            }
            at.MaxAlignmentCount = 1;
            alignments = at.GenAlignments();
            StringBuilder sb = new StringBuilder();
            sb.Append(makeMultiLine(35, "The global alignment score between \""
                + at.Left.Name.Substring(1)
                + "\" and \"" + at.Top.Name.Substring(1)
                + "\" is " + at.MaxScore));
            addLineSeperator(sb);
            addLineSeperator(sb);
            for (int i = 0; i < alignments.Count; i++)
            {
                sb.Append(alignments[i].ToString(35));
                addLineSeperator(sb);
                addLineSeperator(sb);
            }
            return sb.ToString();
        }
        protected internal String getLocalAlignment()
        {
            List<PairWiseAlignment> alignments;
            AlignmentTable at = null;
            if (alphabet.IndelPenalty == int.MaxValue)
            {
                at = new AffineLocalAlignmentTable(sequences[0], sequences[1], alphabet);
            }
            else
            {
                at = new LocalAlignmentTable(sequences[0], sequences[1], alphabet);
            }
            at.MaxAlignmentCount = 1;
            alignments = at.GenAlignments();
            StringBuilder sb = new StringBuilder();
            sb.Append(makeMultiLine(35, "The local alignment score between \""
                + at.Left.Name.Substring(1)
                + "\" and \"" + at.Top.Name.Substring(1)
                + "\" is " + at.MaxScore));
            addLineSeperator(sb);
            addLineSeperator(sb);
            for (int i = 0; i < alignments.Count; i++)
            {
                sb.Append(alignments[i].ToString(35));
                addLineSeperator(sb);
                addLineSeperator(sb);
            }
            return sb.ToString();
        }
        static protected internal String makeMultiLine(int maxCharsPerLine, String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (i % 35 == 0 && i != 0)
                {
                    addLineSeperator(sb);
                }
                sb.Append(s[i]);
            }
            return sb.ToString();
        }

        static protected internal void addLineSeperator(StringBuilder sb)
        {
            sb.Append('\u000A');
            //sb.Append('\u000B');
            //sb.Append('\u000C');
            //sb.Append('\u000D');
            //sb.Append('\u0085');
            //sb.Append('\u2028');
            //sb.Append('\u2029');
        }
    }
}
