/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package genomescanner;

import java.io.FileReader;
import java.util.Scanner;
import genomescanner.ColumnMetaToken.ColumnMetaTokenType;
import java.util.ArrayList;
import java.util.HashMap;
import static genomescanner.VcfDataLineToken.VcfDataLineTokenType;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.Statement;
        
class GenomeScannerException extends Exception{
    public GenomeScannerException(String s){
        super(s);
    }
    
    public GenomeScannerException(int lineNumber, String line, String fileName){
        this("Invalid token found in " + fileName + "\non line " + lineNumber
                + "\naround \"" + line + "\"");
    }

    public GenomeScannerException(int lineNumber, String line, String fileName,
            String expectedToken){
        this("Invalid token found in " + fileName + "\non line " + lineNumber
                + "\naround \"" + line + "\"\nexpected " + expectedToken);
    }

    public GenomeScannerException(int lineNumber, String line, String fileName, VcfDataLineTokenType expectedToken){
        this("Invalid token found in " + fileName + "\non line " + lineNumber
                + "\naround \"" + line + "\"\nexpected "
                + expectedToken.getClass().getName() + "."
                + expectedToken.name() + "\n");
    }

    public static GenomeScannerException create(int lineNumber, String line, String fileName, VcfDataLineTokenType prevTokenType) throws Exception{
        StringBuilder sb = new StringBuilder();
        sb.append("Invalid token found!\nfile = ");
        sb.append(fileName);
        sb.append("\nline number = ");
        sb.append(String.valueOf(lineNumber));
        sb.append("\nline segment = ");
        sb.append(line);
        sb.append("\nprevious token = ");
        sb.append(prevTokenType.name());
        sb.append("\nexpected followers =");
        for(VcfDataLineTokenType follower : prevTokenType.getFollowers()){
            sb.append(" ");
            sb.append(follower.name());
        }
        return new GenomeScannerException(sb.toString());
    }
}

/**
 *
 * @author bill
 */
public class GenomeScanner {
    
    public static int lineCount = 0;
    private static ColumnMetaToken cmt = null;
    private static VcfDataLineToken vdlt = null;
    private static String line = null;
    public static String curFile = null;
    private static int nextId = 0;
    private static Connection con = null;
    private static Statement stmtCmd = null;
    private static Statement stmtQry = null;
    private static ResultSet rs = null;
    private static String sqlStmt = null;
    private static int startLine = 528;

    private static void main() throws Exception{
        main(new String[]{
            "-fileList", "/home/bill/Documents/filesToScan.txt",
            "-outColFile", "/home/bill/Documents/outColFie.txt",
            "-nextId", "1",
            "-uploadMutations"
        });
    }
    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) throws Exception {
        if(args == null || args.length == 0){ main(); return; }
        String fileList = null;
        String outColFile = null;
        boolean extractColumns = false;
        boolean uploadMutations = false;
        for(int i = 0; i < args.length; i++){
            switch(args[i]){
                case "-fileList":
                    fileList = args[++i];
                    break;
                case "-outColFile":
                    outColFile = args[++i];
                    break;
                case "-extractColumns":
                    extractColumns = true;
                    break;
                case "-uploadMutations":
                    uploadMutations = true;
                    break;
                case "-nextId":
                    GenomeScanner.nextId = Integer.parseInt(args[++i]);
                    break;
                case "-startLine":
                    GenomeScanner.startLine = Integer.parseInt(args[++i]);
                    break;
                default:
                    throw new GenomeScannerException("Invalid argument: " + args[i]);
            }
        }
        if (extractColumns){
            extractColumns(fileList, outColFile);
        }
        if(uploadMutations){
            uploadMutations(fileList);
        }
    }
    
    private static void uploadMutations(String fileList) throws Exception{
        Scanner scFiles = null;
        Scanner scGene = null;
        HashMap<String,String> mutRec = new HashMap<>();
        ArrayList<String> alleleCnts = new ArrayList<>();
        ArrayList<String> altBases = new ArrayList<>();
        ArrayList<String> formats = new ArrayList<>();
        ArrayList<ArrayList<String>> formatValues = new ArrayList<>();
        try{
            Class.forName("org.postgresql.Driver");
            con = DriverManager.getConnection("jdbc:postgresql://127.0.0.1:5432/GenomeProcessing", "username", "password");
            stmtCmd = con.createStatement();
            scFiles = new Scanner(new FileReader(fileList));
            while(scFiles.hasNextLine()){
                curFile = scFiles.nextLine();
                String dlDir = curFile.substring(curFile.lastIndexOf('/', curFile.lastIndexOf('/') - 1) + 1, curFile.lastIndexOf('/'));
                String mutType = "1";
                String filePref = null;
                String gene = null;
                String genePart = null;
                try{
                    filePref = curFile.substring(curFile.lastIndexOf('/') + 1, curFile.lastIndexOf(".bam."));
                    gene = curFile.substring(curFile.lastIndexOf(".", curFile.lastIndexOf('_')) + 1, curFile.lastIndexOf('_'));
                    genePart = curFile.substring(curFile.lastIndexOf('_') + 1, curFile.lastIndexOf('.'));
                } catch (Exception e){
                    System.err.println(curFile);
                    System.err.println(e.getMessage());
                    e.printStackTrace(System.err);                   
                    continue;
                }
                scGene = new Scanner(new FileReader(curFile));
                GenomeScanner.lineCount = 0;
                line = null;
                while (scGene.hasNextLine() && (line == null || !line.startsWith("#CHROM"))) {
                    GenomeScanner.lineCount++;
                    line = scGene.nextLine();
                }
                while (GenomeScanner.lineCount < GenomeScanner.startLine){
                    GenomeScanner.lineCount++;
                    line = scGene.nextLine();                    
                }
                GenomeScanner.startLine = 0;
                while (scGene.hasNextLine()) {
                    mutRec.clear();
                    mutRec.put("dlDir", dlDir);
                    mutRec.put("mutType", mutType);
                    mutRec.put("filePref", filePref);
                    mutRec.put("gene", gene);
                    mutRec.put("genePart", genePart);
                    GenomeScanner.lineCount++;
                    line = scGene.nextLine();
                    formats.clear();
                    formatValues.clear();
                    alleleCnts.clear();
                    altBases.clear();
                    int formatPtr = 0;
                    vdlt = VcfDataLineToken.getNext(null, line);
                    while (vdlt.type != VcfDataLineTokenType.END) {
                        line = line.substring(vdlt.anteparseValue.length()).trim();
                        vdlt = VcfDataLineToken.getNext(vdlt, line);
                        String val = "NULL";
                        switch (vdlt.type) {
                            case CHROM:
                                mutRec.put("chromosome", vdlt.getValue());
                                break;
                            case POS:
                                mutRec.put("startPos", vdlt.getValue());
                                break;
                            case ID:
                                if (!vdlt.getValue().trim().equals(".")) {
                                    throw new GenomeScannerException(lineCount,
                                            line, curFile,
                                            VcfDataLineTokenType.ID);
                                }
                                break;
                            case REF:
                                mutRec.put("refBases", vdlt.getValue());
                                break;
                            case ALT:
                                altBases.add(vdlt.getValue());
                                break;
                            case QUAL:
                                mutRec.put("quality", vdlt.getValue());
                                break;
                            case FILTER:
                                if (!vdlt.getValue().trim().equals(".")) {
                                    throw new GenomeScannerException(lineCount,
                                            line, curFile,
                                            VcfDataLineTokenType.FILTER);
                                }
                                break;
                            case INFO_DP:
                                mutRec.put("INFO_DP_RawReadDepth",
                                        getSingleInfoValue());
                                break;
                            case INFO_MQ:
                                mutRec.put("INFO_MQ_RootMeanSqrMapQualCoveringReads",
                                        getSingleInfoValue());
                                break;
                            case INFO_FQ:
                                mutRec.put("INFO_FQ_PhredProbAllSamplesSame",
                                        getSingleInfoValue());
                                break;
                            case INFO_AF1:
                                mutRec.put("INFO_AF1_MaxLikelihoodEstFirstAltAlleleFreq",
                                        getSingleInfoValue());
                                break;
                            case INFO_AC1:
                                mutRec.put("INFO_AC1_MaxLikelihoodEstFirstAltAlleleCnt",
                                        getSingleInfoValue());
                                break;
                            case INFO_AN:
                                mutRec.put("INFO_AN_TotNumAllelesInCalledGenotypes",
                                        getSingleInfoValue());
                                break;
                            case INFO_HWE:
                                mutRec.put("INFO_HWE_Chi2BasedHweTestPValFromG3",
                                        getSingleInfoValue());
                                break;
                            case INFO_CLR:
                                mutRec.put("INFO_CLR_LogRatioGenotypeLikelihoods",
                                        getSingleInfoValue());
                                break;
                            case INFO_UGT:
                                mutRec.put("INFO_UGT_MostProbableGenotypeConf",
                                        getSingleInfoValue());
                                break;
                            case INFO_CGT:
                                mutRec.put("INFO_CGT_MostProbableGenotypeConf",
                                        getSingleInfoValue());
                                break;
                            case INFO_PCHI2:
                                mutRec.put("INFO_PCHI2_PostWeightedChi2PValGroup1Group2",
                                        getSingleInfoValue());
                                break;
                            case INFO_QCHI2:
                                mutRec.put("INFO_QCHI2_PhredScaledPChi2",
                                        getSingleInfoValue());
                                break;
                            case INFO_PR:
                                mutRec.put("INFO_PR_NumPermutationsYieldingSmallerPChi2",
                                        getSingleInfoValue());
                                break;
                            case INFO_QBD:
                                mutRec.put("INFO_QBD_QualityOverDepth",
                                        getSingleInfoValue());
                                break;
                            case INFO_RPB:
                                mutRec.put("INFO_RPB_ReadPositionBias",
                                        getSingleInfoValue());
                                break;
                            case INFO_MDV:
                                mutRec.put("INFO_MDV_MaxNumHighQualNonRefReads",
                                        getSingleInfoValue());
                                break;
                            case INFO_VDB:
                                mutRec.put("INFO_VDB_VariantDistanceBias",
                                        getSingleInfoValue());
                                break;
                            case FORMAT_GT:
                                formats.add("FORMAT_GT_Genotype");
                                formatValues.add(new ArrayList<>());
                                break;
                            case FORMAT_GQ:
                                formats.add("FORMAT_GQ_GenotypeQuality");
                                formatValues.add(new ArrayList<>());
                                break;
                            case FORMAT_DP:
                                formats.add("FORMAT_DP_NumHighQualityBases");
                                formatValues.add(new ArrayList<>());
                                break;
                            case FORMAT_DV:
                                formats.add("FORMAT_DV_NumHighQualityNonRefBases");
                                formatValues.add(new ArrayList<>());
                                break;
                            case FORMAT_SP:
                                formats.add("FORMAT_SP_PhredScaledStrandBiasPVal");
                                formatValues.add(new ArrayList<>());
                                break;
                            case INFO_DP4:
                                mutRec.put("INFO_DP4_NumHighQualRefFwdBases",
                                        getValue(VcfDataLineTokenType.INFO_EQUALS,
                                                VcfDataLineTokenType.INFO_VAL_STRING));
                                mutRec.put("INFO_DP4_NumHighQualRefRevBases",
                                        getValue(VcfDataLineTokenType.INFO_VAL_SEP_COMMA,
                                                VcfDataLineTokenType.INFO_VAL_STRING));
                                mutRec.put("INFO_DP4_NumHighQualAltFwdBases",
                                        getValue(VcfDataLineTokenType.INFO_VAL_SEP_COMMA,
                                                VcfDataLineTokenType.INFO_VAL_STRING));
                                mutRec.put("INFO_DP4_NumHighQualAltRevBases",
                                        getValue(VcfDataLineTokenType.INFO_VAL_SEP_COMMA,
                                                VcfDataLineTokenType.INFO_VAL_STRING));
                                break;
                            case INFO_INDEL:
                                mutRec.put("mutType", "2");
                                break;
                            case INFO_IS:
                                mutRec.put("INFO_IS_MaxNumReadsSupportingIndel",
                                        getValue(VcfDataLineTokenType.INFO_EQUALS,
                                                VcfDataLineTokenType.INFO_VAL_STRING));
                                mutRec.put("INFO_IS_FractionOfIndelReads",
                                        getValue(VcfDataLineTokenType.INFO_VAL_SEP_COMMA,
                                                VcfDataLineTokenType.INFO_VAL_STRING));
                                break;
                            case INFO_G3:
                                mutRec.put("INFO_G3_MaxLikelihoodEstGenotypeFreqA1A1",
                                        getValue(VcfDataLineTokenType.INFO_EQUALS,
                                                VcfDataLineTokenType.INFO_VAL_STRING));
                                mutRec.put("INFO_G3_MaxLikelihoodEstGenotypeFreqA1A2",
                                        getValue(VcfDataLineTokenType.INFO_VAL_SEP_COMMA,
                                                VcfDataLineTokenType.INFO_VAL_STRING));
                                mutRec.put("INFO_G3_MaxLikelihoodEstGenotypeFreqA2A2",
                                        getValue(VcfDataLineTokenType.INFO_VAL_SEP_COMMA,
                                                VcfDataLineTokenType.INFO_VAL_STRING));
                                break;
                            case INFO_PV4:
                                mutRec.put("INFO_PV4_PValStrandBias",
                                        getValue(VcfDataLineTokenType.INFO_EQUALS,
                                                VcfDataLineTokenType.INFO_VAL_STRING));
                                mutRec.put("INFO_PV4_PValBaseQBias",
                                        getValue(VcfDataLineTokenType.INFO_VAL_SEP_COMMA,
                                                VcfDataLineTokenType.INFO_VAL_STRING));
                                mutRec.put("INFO_PV4_PValMapQBias",
                                        getValue(VcfDataLineTokenType.INFO_VAL_SEP_COMMA,
                                                VcfDataLineTokenType.INFO_VAL_STRING));
                                mutRec.put("INFO_PV4_PValTailDistanceBias",
                                        getValue(VcfDataLineTokenType.INFO_VAL_SEP_COMMA,
                                                VcfDataLineTokenType.INFO_VAL_STRING));
                                break;
                            case INFO_PC2:
                                mutRec.put("INFO_PC2_PhredProbNonRefAlleleFreqGroup1LargerGroup2",
                                        getValue(VcfDataLineTokenType.INFO_EQUALS,
                                                VcfDataLineTokenType.INFO_VAL_STRING));
                                mutRec.put("INFO_PC2_PhredProbNonRefAlleleFreqGroup1SmallerGroup2",
                                        getValue(VcfDataLineTokenType.INFO_VAL_SEP_COMMA,
                                                VcfDataLineTokenType.INFO_VAL_STRING));
                                break;
                            case FORMAT_GL:
                                formats.add("INFO_GL");
                                formatValues.add(new ArrayList<>());
                                break;
                            case INFO_AC:
                                alleleCnts.add(getValue(VcfDataLineTokenType.INFO_EQUALS,
                                        VcfDataLineTokenType.INFO_VAL_STRING));
                                while (true) {
                                    vdlt = VcfDataLineToken.getNext(vdlt, line);
                                    line = line.substring(vdlt.anteparseValue.length());
                                    if (vdlt.type != VcfDataLineTokenType.INFO_VAL_SEP_COMMA) {
                                        break;
                                    }
                                    alleleCnts.add(getValue(VcfDataLineTokenType.INFO_EQUALS));
                                }
                                break;
                            case FORMAT_PL:
                                formats.add("FORMAT_PL_PhredScaledGenotypeLikelihood");
                                formatValues.add(new ArrayList<>());
                                break;
                            case FORMAT_VAL_STRING:
                                formatValues.get(formatPtr).add(vdlt.getValue());
                                break;
                            case FORMAT_VAL_SEP_COLON:
                                formatPtr++;
                                break;
                            default:
                                continue;
                        }
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.append("INSERT INTO tblMutation \n(");
                    String[] cols = mutRec.keySet().toArray(new String[]{});
                    boolean first = true;
                    for (String col : cols) {
                        if (!first) {
                            sb.append(',');
                        }
                        sb.append(col);
                        first = false;
                    }
                    for (String col : formats) {
                        if (col.equals("FORMAT_PL_PhredScaledGenotypeLikelihood")) {
                            continue;
                        }
                        if (col.equals("INFO_GL")) {
                            if (!first) {
                                sb.append(',');
                            }
                            sb.append("INFO_GL_LikelihoodOfRefRefGenotype,");
                            sb.append("INFO_GL_LikelihoodOfRefAltGenotype,");
                            sb.append("INFO_GL_LikelihoodOfAltAltGenotype");
                            first = false;
                            continue;
                        }
                        if (!first) {
                            sb.append(',');
                        }
                        sb.append(col);
                        first = false;
                    }
                    sb.append(")\nVALUES\n(");
                    first = true;
                    for (String col : cols) {
                        if (!first) {
                            sb.append(',');
                        }
                        sb.append('\'');
                        sb.append(mutRec.get(col));
                        sb.append('\'');
                        first = false;
                    }
                    for (int i = 0; i < formats.size(); i++) {
                        if (formats.get(i).equals("FORMAT_PL_PhredScaledGenotypeLikelihood")) {
                            continue;
                        }
                        for (String val : formatValues.get(i)) {
                            if (!first) {
                                sb.append(',');
                            }
                            sb.append('\'');
                            sb.append(val);
                            sb.append('\'');
                            first = false;
                        }
                    }
                    sb.append(");\n");
                    for (int i = 0; i < formats.size(); i++) {
                        if (!formats.get(i).equals("FORMAT_PL_PhredScaledGenotypeLikelihood")) {
                            continue;
                        }
                        if (formatValues.get(i).isEmpty()){
                            break;
                        }
                        sb.append("INSERT INTO tblPhredScaledGenotypeLikelihood\n(");
                        sb.append("FORMAT_PL_PhredScaledGenotypeLikelihood,");
                        sb.append("tblMutation_id_key)\nVALUES\n");
                        first = true;
                        for (String val : formatValues.get(i)) {
                            if(!first){
                                sb.append(",\n");
                            }
                            sb.append("('");
                            sb.append(val);
                            sb.append("','");
                            sb.append(String.valueOf(nextId));
                            sb.append("')");
                            first = false;
                        }
                        sb.append(";\n");
                    }
                    if (!alleleCnts.isEmpty()){
                        sb.append("INSERT INTO tblAlleleCnt\n(");
                        sb.append("INFO_AC_AlleleCntInGenotypes,");
                        sb.append("tblMutation_id_key)\nVALUES\n");
                        first = true;
                        for (String val : alleleCnts) {
                            if(!first){
                                sb.append(",\n");
                            }
                            sb.append("('");
                            sb.append(val);
                            sb.append("','");
                            sb.append(String.valueOf(nextId));
                            sb.append("')");
                            first = false;
                        }
                        sb.append(";\n");                        
                    }
                    if (!altBases.isEmpty()){
                        sb.append("INSERT INTO tblALtBases\n(");
                        sb.append("altBases,");
                        sb.append("tblMutation_id_key)\nVALUES\n");
                        first = true;
                        for (String val : altBases) {
                            if(!first){
                                sb.append(",\n");
                            }
                            sb.append("('");
                            sb.append(val);
                            sb.append("','");
                            sb.append(String.valueOf(nextId));
                            sb.append("')");
                            first = false;
                        }
                        sb.append(";\n");                        
                    }
                    //System.out.println(sb.toString());
                    sqlStmt = sb.toString();
                    stmtCmd.execute(sqlStmt);
                    sqlStmt = null;
                    nextId++;
                }
                scGene.close();
            }
            scFiles.close();
            stmtCmd.close();
            con.close();
        } catch (Exception e) {
            System.out.println("file = " + curFile);
            System.out.println("\nline number = " + lineCount);
            if (sqlStmt != null){
                System.err.println(sqlStmt);
            }
            throw e;
        } finally {
            try {
                scFiles.close();
            } catch (Exception dontcare) {
            }
            try {
                scGene.close();
            } catch (Exception dontcare) {
            }
            try {
                stmtCmd.close();
            } catch (Exception dontCare) {
            }
            try {
                con.close();
            } catch (Exception dontCare) {
            }
        }
    }

    private static void extractColumns(String fileList, String outFile) throws Exception {
        Scanner scFiles = null;
        Scanner scGene = null;
        try {
            scFiles = new Scanner(new FileReader(fileList));
            while(scFiles.hasNextLine()){
                String fullFileName = scFiles.nextLine();
                String baseFileName = fullFileName.substring(fullFileName.lastIndexOf('/') + 1, fullFileName.lastIndexOf(".bam."));
                String folder = fullFileName.substring(fullFileName.lastIndexOf('/', fullFileName.lastIndexOf('/') - 1) + 1, fullFileName.lastIndexOf('/'));
                String gene = fullFileName.substring(fullFileName.lastIndexOf(".", fullFileName.lastIndexOf('_')) + 1, fullFileName.lastIndexOf('_'));
                String genePart = fullFileName.substring(fullFileName.lastIndexOf('_') + 1, fullFileName.lastIndexOf('.'));
                scGene = new Scanner(new FileReader(scFiles.nextLine()));
                GenomeScanner.lineCount = 0;
                while(scGene.hasNextLine()){
                    GenomeScanner.lineCount++;
                    line = scGene.nextLine();
                    if (line.startsWith("##INFO") || line.startsWith("##FORMAT")){
                        Column col = new Column();
                        cmt = null;
                        while(cmt == null || cmt.type != ColumnMetaTokenType.CLOSE){
                            cmt = ColumnMetaToken.getNext(cmt, line);
                            line = line.substring(cmt.anteparseValue.length());
                            switch(cmt.type){
                                case GROUP:
                                    col.group = cmt.anteparseValue;
                                    break;
                                case ID:
                                    col.id = getColumnMetaValue();
                                    break;
                                case NUMBER:
                                    col.number = getColumnMetaValue();
                                    break;
                                case TYPE:
                                    col.type = getColumnMetaValue();
                                    break;
                                case DESCRIPTION:
                                    col.desc = getColumnMetaValue();
                                    break;
                            }
                        }
                        Column.add(col);
                    }
                }
                scGene.close();
                Column.generateRegex(outFile);
                //Column.generateSql(outColFile);
                break;
            }
            scFiles.close();
        } catch (Exception e){
            throw e;
        } finally{
            try{scFiles.close();}catch(Exception dontcare){}
            try{scGene.close();}catch(Exception dontcare){}
        }        
    }

    private static String getValue(VcfDataLineTokenType... expectedTokenSequence)
            throws Exception{
        for (VcfDataLineTokenType t : expectedTokenSequence) {
            line = line.substring(vdlt.anteparseValue.length());
            vdlt = VcfDataLineToken.getNext(vdlt, line);
            if (vdlt.type != t) {
                throw new GenomeScannerException(lineCount, line, curFile, t);
            }
        }
        return vdlt.getValue();
    }
    
    private static String getSingleInfoValue() throws Exception{
        return getValue(VcfDataLineTokenType.INFO_EQUALS,
                VcfDataLineTokenType.INFO_VAL_STRING);
    }
        
    private static void throwInvalidColumnMetaToken()
            throws GenomeScannerException{
        throw new GenomeScannerException("Invalid token:\n" + cmt.anteparseValue
                + "\naround\n" + line);
    }
    
    private static String getColumnMetaValue() throws Exception{
        cmt = ColumnMetaToken.getNext(cmt, line);
        line = line.substring(cmt.anteparseValue.length());
        if(cmt.type != ColumnMetaTokenType.EQUALS){
            throwInvalidColumnMetaToken();
        }
        cmt = ColumnMetaToken.getNext(cmt, line);
        line = line.substring(cmt.anteparseValue.length());
        if(cmt.type != ColumnMetaTokenType.VALUE){
            throwInvalidColumnMetaToken();
        }
        return cmt.anteparseValue;
    }
}