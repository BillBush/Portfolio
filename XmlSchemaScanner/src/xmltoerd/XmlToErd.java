package xmltoerd;

import java.io.FileReader;
import java.io.FileWriter;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.EnumMap;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Scanner;
import xmltoerd.Column.ColumnType;
import static xmltoerd.Column.ColumnType.*;
import static xmltoerd.XmlToken.TokenType.*;

class XmlToken{//<editor-fold desc="..." defaultstate="collapsed">
    public enum TokenType {
        CLOSE_TAG_OPEN("</"),
        CLOSE_TAG_NAME("[ ]*[A-Za-z0-9_:-][A-Za-z0-9_:-]*[ ]*"),
        CLOSE_TAG_CLOSE("[ ]*>[ ]*"),
        PROP_VAL("..*</"),
        PROP_CLOSE_TAG("[ ]*/[ ]*>[ ]*"),
        PROP_CLOSE("[ ]*>[ ]*"),
        ATTR_VAL("([ ]*\'[^\'][^\']*\'[ ]*|[ ]*\'[^\']*\'[ ]*|[ ]*\"[^\"][^\"]*\"[ ]*|[ ]*\"[^\"]*\"[ ]*)"),
        ATTR_EQ("[ ]*=[ ]*"),
        ATTR_NAME("[ ]*[A-Za-z0-9_:-][A-Za-z0-9_:-]*[ ]*"),
        PROP_NAME("[ ]*[A-Za-z0-9_:-][A-Za-z0-9_:-]*[ ]*"),
        PROP_OPEN("[ ]*<[ ]*");
        
        private String regex;
        private static final EnumMap<TokenType, TokenType[]> followers;
        
        static{
            followers = new EnumMap<>(TokenType.class);
            followers.put(CLOSE_TAG_OPEN, new TokenType[]{CLOSE_TAG_NAME});
            followers.put(CLOSE_TAG_NAME, new TokenType[]{CLOSE_TAG_CLOSE});
            followers.put(CLOSE_TAG_CLOSE, new TokenType[]{CLOSE_TAG_OPEN, PROP_OPEN});
            followers.put(PROP_VAL, new TokenType[]{CLOSE_TAG_OPEN, PROP_OPEN});
            followers.put(PROP_CLOSE_TAG, new TokenType[]{CLOSE_TAG_OPEN, PROP_OPEN});
            followers.put(PROP_CLOSE, new TokenType[]{CLOSE_TAG_OPEN, PROP_OPEN, PROP_VAL});
            followers.put(ATTR_VAL, new TokenType[]{ATTR_NAME, PROP_CLOSE, PROP_CLOSE_TAG});
            followers.put(ATTR_EQ, new TokenType[]{ATTR_VAL});
            followers.put(ATTR_NAME, new TokenType[]{ATTR_EQ});
            followers.put(PROP_NAME, new TokenType[]{PROP_CLOSE, PROP_CLOSE_TAG, ATTR_NAME});
            followers.put(PROP_OPEN, new TokenType[]{PROP_NAME});
        }
        
        TokenType(String regex){
            this.regex = regex;
        }
        
        TokenType[] getFollowers() throws Exception{
            return followers.get(this);
        }
        
        TokenType getNextType(String line) throws Exception{
            line = line.trim();
            if (!line.isEmpty()){
                for (TokenType follower : getFollowers()){
                    if(line.matches(follower.regex + ".*")){
                        return follower;
                    }
                }
                throw new Exception("Invalid token following " + this + " at "+ XmlToErd.lineCount + ": "+ line);
            }
            return null;
        }
        
        String getValue(String line) throws Exception{
            line = line.trim();
            int length = line.length();
            if (!line.isEmpty()){
                switch(this){
                    case PROP_VAL:
                        return line.replaceAll("</..*>.*", "");
                    default:
                        length -= line.replaceFirst(regex, "").length();
                        return line.substring(0, length);
                }
            }
            throw new Exception("no value defined for token, " + name() + ", in " + line);
        }
    }
    
    public final TokenType type;
    public final String anteparseValue;
    private static String wkString;
    
    private XmlToken(TokenType type, String anteparseValue){
        this.type = type;
        this.anteparseValue = anteparseValue;
    }

    public String getValue() {
        switch (type){
            case ATTR_VAL:
                wkString = anteparseValue.trim();
                return wkString.substring(0, wkString.length() - 1).substring(1).trim();
            case PROP_NAME:
            case CLOSE_TAG_NAME:
            case ATTR_NAME:
                return anteparseValue.replaceAll(":", "_")
                        //.replaceAll("_","")
                        .replaceAll("[Rr][Ee][Pp][Oo][Rr][Tt]","rpt")
                        .replaceAll("[Ss][Aa][Mm][Pp][Ll][Ee]","smpl")
                        .replaceAll("[Cc][Oo][Nn][Tt][Rr][Oo][Ll]","ctrl")
                        .replaceAll("[Ss][Hh][Ii][Pp][Mm][Ee][Nn][Tt]","shpmt")
                        .replaceAll("[Cc][Hh][Ee][Cc][Kk]","ck")
                        .replaceAll("[Mm][Ee][Tt][Hh][Oo][Dd]","mthd")
                        .replaceAll("[Pp][Aa][Tt][Ii][Ee][Nn][Tt]","pat")
                        .replaceAll("[Ss][Tt][Aa][Tt][Uu][Ss]","status")
                        .replaceAll("[Pp][Oo][Rr][Tt][Ii][Oo][Nn]","portn")
                        .replaceAll("[Yy][Ee][Aa][Rr]","yr")
                        .replaceAll("[Mm][Oo][Nn][Tt][Hh]","mo")
                        .replaceAll("[Dd][Aa][Yy]","dy")
                        .replaceAll("[Rr][Ee][Aa][Ss][Oo][Nn][Ss]","rsns")
                        .replaceAll("[Rr][Ee][Aa][Ss][Oo][Nn]","rsn")
                        .replaceAll("[Nn][Oo][Rr][Mm][Aa][Ll]","norm")
                        .replaceAll("[Cc][Aa][Nn][Oo][Nn][Ii][Cc][Aa][Ll]","cnncl")
                        .replaceAll("[Bb][Ii][Oo][Ss][Pp][Ee][Cc][Ii][Mm][Ee][Nn]","biospcmn")
                        .replaceAll("[Ss][Uu][Bb][Mm][Ii][Tt][Tt][Ee][Dd]","sbmtd")
                        .replaceAll("[Ss][Uu][Bb][Mm][Ii][Tt]","sbmt")
                        .replaceAll("[Pp][Rr][Oo][Cc][Uu][Rr][Ee][Mm][Ee][Nn][Tt]","prcrmt")
                        .replaceAll("[Cc][Oo][Nn][Ff][Ii][Rr][Mm]","cnfrm")
                        .replaceAll("[Dd][Ii][Ff][Ff][Ee][Rr][Ee][Nn][Tt][Ii][Aa][Ll]","dffrntl")
                        .replaceAll("[Pp][Ee][Rr][Cc][Ee][Nn][Tt]","prcnt")
                        .replaceAll("[Mm][Aa][Tt][Cc][Hh][Ii][Nn][Gg]","mtchng")
                        .replaceAll("[Mm][Aa][Tt][Cc][Hh][Ee][Ss]","mtchs")
                        .replaceAll("[Mm][Aa][Tt][Cc][Hh]","mtch")
                        .replaceAll("[Ss][Pp][Ee][Cc][Ii][Mm][Ee][Nn]","spcmn")
                        .replaceAll("[Pp][Aa][Tt][Hh]","pth")
                        .replaceAll("[Dd][Ii][Aa][Gg][Nn][Oo][Ss][Ii][Ss]","diag")
                        .replaceAll("[Tt][Uu][Mm][Oo][Rr]","tmr")
                        .replaceAll("[Nn][Ee][Cc][Rr][Oo][Ss][Ii][Ss]","necro")
                        .replaceAll("[Mm][Ee][Tt][Rr][Ii][Cc][Ss]","mtrcs")
                        .replaceAll("[Ss][Hh][Aa][Rr][Ee][Dd]","shrd")
                        .replaceAll("[Hh][Ii][Ss][Tt][Oo][Rr][Yy]","hstry")
                        .replaceAll("[Tt][Rr][Ee][Aa][Tt][Mm][Ee][Nn][Tt]","trtmnt")
                        .replaceAll("[Mm][Yy][Ee][Ll][Oo][Bb][Ll][Aa][Ss][Tt][Ss]","mylblsts")
//                        .replaceAll("","")
//                        .replaceAll("","")
                        .trim();
            default:
                return anteparseValue.trim();
        }
    }
    
    private XmlToken getNext(String line) throws Exception{
        line = line.trim();
        if (!line.isEmpty()){
            TokenType nextType = type.getNextType(line);
            String nextValue = nextType.getValue(line);
            return new XmlToken(nextType, nextValue);
        }
        return null;
    }
    
    public static XmlToken getNext(XmlToken prev, String line) throws Exception {
        line = line.trim();
        if (!line.isEmpty()){
            if (prev == null){
                return new XmlToken(PROP_OPEN, PROP_OPEN.getValue(line));
            } else {
                return prev.getNext(line);
            }
        }
        return null;
    }
    
    @Override
    public String toString() {
        return "XmlToken:"
                + " type=" + type.name()
                + ", anteparseValue=" + anteparseValue
                + ", value=" + getValue();
    }
/*</editor-fold>*/}

class Column{//<editor-fold desc="..." defaultstate="collapsed">
    public enum ColumnType{
        BIGINT("(-|[+]|)[0-9][0-9]*"),
        NUMERIC("(-|[+]|)[0-9][0-9]*[.][0-9][0-9]*"),
        DATE("[1-2][189][0-9][0-9](-[0-9][0-9]-[0-9][0-9]|-[0-9][0-9]|)"),
        DATETIME("[1-2][189][0-9][0-9](-[0-9][0-9]-[0-9][0-9]|-[0-9][0-9]|)([+]| |_|)([0-9][0-9]:[0-9][0-9]|)"),
        VARCHAR(".*");
        
        String regex;
        
        ColumnType(String regex){
            this.regex = regex;
        }
        
        public String getValueForInsert(String value){
            value = value.trim();
            if (this == DATETIME){
                value = value.replaceAll("[+]", " ");
            }
            value = value.replaceAll("'", "''");
            return value;
        }
        
        public static ColumnType getColumnType(String value) throws Exception{
            if (value.matches(BIGINT.regex)){
                return BIGINT;
            } else if (value.matches(NUMERIC.regex)){
                return NUMERIC;
            } else if (value.matches(DATE.regex)){
                return DATE;
            } else if (value.matches(DATETIME.regex)){
                return DATETIME;
            } else if (value.matches(VARCHAR.regex)){
                return VARCHAR;
            }
            throw new Exception("ColumnType could not be determined for \"" + value + "\"");
        }
        
        public static ColumnType getMinSuperType(Column c1, Column c2) throws Exception{
            if (c1.type == c2.type){
                return c1.type;
            }
            if ((c1.type == BIGINT || c1.type == NUMERIC) && (c2.type == BIGINT || c2.type == NUMERIC)){
                return NUMERIC;
            }
            if ((c1.type == DATE || c1.type == DATETIME) && (c2.type == DATE || c2.type == DATETIME)){
                return DATETIME;
            }
            if ((c1.type == BIGINT || c1.type == DATE || c1.type == DATETIME) && (c2.type == BIGINT || c2.type == DATE || c2.type == DATETIME)){
                if (c1.type == BIGINT && c1.maxScale == 4){
                    return c2.type;
                }
                if (c2.type == BIGINT && c2.maxScale == 4){
                    return c1.type;
                }
            }
            return VARCHAR;
        }
    }

    public String name;
    public ColumnType type;
    public int maxScale = 0, maxPrecision = 0, maxLength = 0;
    
    public Column(String name) throws Exception {
        this.name = name;
    }
    
    private Column(String name, String value) throws Exception{
        this(name);
        updateTypeAndSize(value);
    }
    
    public void updateTypeAndSize(String value) throws Exception{
        if (type == null){
            type = ColumnType.getColumnType(value);
        } else {
            type = ColumnType.getMinSuperType(this, new Column(null, value));
        }
        maxLength = Math.max(maxLength, value.length() + (50 - (value.length() % 50)));
        maxScale = Math.max(maxScale, value.length());
        maxPrecision = Math.max(maxPrecision, value.replaceFirst("[^.]*\\.", "").length());
    }
    
/*</editor-fold>*/}

class Table{//<editor-fold desc="..." defaultstate="collapsed">
    public static final HashMap<String, Table> tables = new HashMap<>();
    public static Table root;
    public final HashSet<Table> references = new HashSet<>();
    public final HashMap<String, Column> columns = new HashMap<>();
    public final String name;
    private static final HashSet<Table> droped = new HashSet<>();
    private int serial = 0;
    
    private Table(String name) throws Exception{
        this.name = name;
    }
    
    public Column getColumn(String name) throws Exception{
        Column column = columns.get(name);
        if (column == null){
            column = new Column(name);
            columns.put(name, column);
        }
        return column;
    }
    
    public static Table getTable(String name) throws Exception{
        Table table = tables.get(name);
        if (table == null){
            table = new Table(name);
            tables.put(name, table);
        }
        return table;
    }
    
    protected String getTableName(){
        String firstChar = name.substring(0,1).toUpperCase();
        return "tbl" + firstChar + name.substring(1);
    }
    
    public String getSerialSeqName(){
        return getTableName() + "_id_seq";
    }
    
    public String getIdColumnName(){
        return getTableName() + "_id_key";
    }
    
    public String getColumnName(Column column){
        return getTableName() + "_" + column.name.replaceAll(":", "__");
    }
    
    public String getCreate(){
        StringBuilder sb = new StringBuilder();
        sb.append("CREATE SEQUENCE ");
        sb.append(getSerialSeqName());
        sb.append(";\n");
        sb.append("CREATE TABLE ");
        sb.append(getTableName());
        sb.append("(\n ");
        sb.append(getIdColumnName());
        sb.append(" INTEGER DEFAULT nextval('");
        sb.append(getSerialSeqName());
        sb.append("') PRIMARY KEY");
        for (Column column : columns.values()){
            sb.append(",\n ");
            sb.append(getColumnName(column));
            sb.append(" ");
            sb.append(column.type.name());
            switch(column.type){
                case NUMERIC:
                    sb.append("(");
                    sb.append(String.valueOf(column.maxScale));
                    sb.append(",");
                    sb.append(String.valueOf(column.maxPrecision));
                    sb.append(")");
                    break;
                case VARCHAR:
                    sb.append("(");
                    sb.append(String.valueOf(column.maxLength));
                    sb.append(")");
                    break;
            }
        }
        sb.append("\n);\n");
        return sb.toString();
    }
    
    public String getForeignKeys(){
        StringBuilder sb = new StringBuilder();
        for (Table table : references.toArray(new Table[0])){
            sb.append("\nALTER TABLE ");
            sb.append(table.getTableName());
            sb.append("\nADD ");
            sb.append(getIdColumnName());
            sb.append(" INTEGER NULL;\n");
            sb.append("ALTER TABLE ");
            sb.append(table.getTableName());
            sb.append("\nADD CONSTRAINT fk_");
            sb.append(table.getTableName());
            sb.append("_");
            sb.append(getTableName());
            sb.append("\nFOREIGN KEY (");
            sb.append(getIdColumnName());
            sb.append(")\nREFERENCES ");
            sb.append(getTableName());
            sb.append("(");
            sb.append(getIdColumnName());
            sb.append(");\n");
        }
        return sb.toString();
    }
    
    public static String getDrop(){
        StringBuilder sb = new StringBuilder();
        droped.clear();
        while(!droped.containsAll(tables.values())){
            for (Table table : tables.values()){
                if ((table.references.size() == 0 || droped.containsAll(table.references)) && !droped.contains(table)){
                    sb.append("\nDROP TABLE ");
                    sb.append(table.getTableName());
                    sb.append(";");
                    sb.append("\nDROP SEQUENCE ");
                    sb.append(table.getSerialSeqName());
                    sb.append(";");
                    droped.add(table);
                }
            }
        }
        return sb.toString();
    }
    
    protected int getIncrementSerial(){
        return (++serial);
    }
    
    @Override
    public String toString() {
        StringBuilder sb = new StringBuilder();
        sb.append("TABLE\n");
        sb.append(name);
        sb.append("\n");
        for (Column column : columns.values()){
            sb.append("COLUMN\n");
            sb.append(column.name);
            sb.append("\nTYPE\n");
            sb.append(column.type.name());
            sb.append("\nMAXLENGTH\n");
            sb.append(String.valueOf(column.maxLength));
            sb.append("\nMAXSCALE\n");
            sb.append(String.valueOf(column.maxScale));
            sb.append("\nMAXPRECISION\n");
            sb.append(String.valueOf(column.maxPrecision));
            sb.append("\n");
        }
        for (Table table : references.toArray(new Table[0])){
            sb.append("REFERENCE\n");
            sb.append(table.name);
            sb.append("\n");
        }
        return sb.toString();
    }
    
/*</editor-fold>*/}

class TableRecord{//<editor-fold desc="..." defaultstate="collapsed">
    protected ArrayList<TableRecord> references = new ArrayList<>();
    protected HashMap<String, String> fields = new HashMap<>();
    protected Table table = null;
    private String serial = null;
    
    public TableRecord(Table table){
        this.table = table;
        this.serial = String.valueOf(table.getIncrementSerial());
    }
    
    public void clear(){
        for (TableRecord reference : references){
            reference.clear();
        }        
        table = null;
        serial = null;
        fields.clear();
        references.clear();
    }
    
    public String getInsert() throws Exception {
        StringBuilder sb = new StringBuilder();
        sb.append("INSERT INTO ");
        sb.append(table.getTableName());
        sb.append(" (");
        String[] columns = fields.keySet().toArray(new String[0]);
        for (int i = 0; i < columns.length; i++){
            if (i != 0){
                sb.append(", ");
            }
            sb.append(columns[i]);
        }
        sb.append(")\nVALUES ('");
        for (int i = 0; i < columns.length; i++){
            if (i != 0){
                sb.append("', '");
            }
            sb.append(fields.get(columns[i]).trim());
        }
        sb.append("');\n");
        for (TableRecord record : references){
            record.fields.put(table.getIdColumnName(), serial);
        }
        for (TableRecord record : references){
            sb.append(record.getInsert());
        }
        return sb.toString();
    }
/*</editor-fold>*/}

public class XmlToErd {
    //<editor-fold desc="globals" defaultstate="collapsed">
    private static byte[] inBuffer = new byte[50];
    private static String line = null;
    private static XmlToken currToken = null;
    private static Scanner sc = null;
    private static FileWriter out = null;
    private static Column wkColumn = null;
    private static Table wkTable = null;
    private static String wkString = null;
    private static Table root = null;
    private static String drivingTable = "bio_tcga_bcr";
    private static int count = 0, limit = 0;
    private static String wkSql = null;
    private static String xmlInFile = "clinical.xml";
    private static String tableOutFile = "clinicalTables.txt";
    private static String tableInFile = tableOutFile;
    private static String tableSqlFile = "clinicalTables.sql";
    private static String testOutFile = "clinicalTest.sql";
    private static boolean parseXml = false;
    private static boolean genTableScript = false;
    private static boolean insertData = true;
    private static boolean outputDetails = false;
    private static boolean test = false;
    private static boolean updateDb = false;
    public static int lineCount = 0;
    public static int insertCount = 0;
    private static Connection con = null;
    private static Statement stmtCmd = null;
    private static Statement stmtQry = null;
    private static ResultSet rs = null;
    //</editor-fold>
    public static void main(String[] args) {
        try {
            //<editor-fold desc="process command line args" defaultstate="collapsed">
            for (int i = 0; i < args.length; i++){
                switch(args[i]){
                    case "-drivingTable":
                        drivingTable = args[++i];
                        break;
                    case "-limit":
                        limit = Integer.parseInt(args[++i]);
                        break;
                    case "-xmlInFile":
                        xmlInFile = args[++i];
                        break;
                    case "-tableOutFile":
                        tableOutFile = args[++i];
                        break;
                    case "-tableInFile":
                        tableInFile = args[++i];
                        break;
                    case "-tableSqlFile":
                        tableSqlFile = args[++i];
                        break;
                    case "-parseXml":
                        parseXml = true;
                        break;
                    case "-genTableScript":
                        genTableScript = true;
                        break;
                    case "-insertData":
                        insertData = true;
                        break;
                    case "-outputDetails":
                        outputDetails = true;
                        break;
                    case "-test":
                        test = true;
                        break;
                    case "testOutFile":
                        testOutFile = args[++i];
                        break;
                    default:
                        throw new Exception("Invalid command line argument: " + args[i]);
                }
            }
            if (test){
                out = new FileWriter(testOutFile, false);
                out.close();
            }
            //</editor-fold>
            //<editor-fold desc="extract table definitions from xml file" defaultstate="collapsed">
            if (parseXml) {
                sc = new Scanner(new FileReader(xmlInFile));
//                System.out.println(sc.nextLine());
                root = Table.getTable("clinicalRoot");
                parseXml(root);
                sc.close();
                
                out = new FileWriter(tableOutFile, false);
                for (Table table : Table.tables.values()) {
                    out.write(table.toString());
                }
                out.close();
            }
            //</editor-fold>
            //<editor-fold desc="load tables from tables file" defaultstate="collapsed">
            if (genTableScript || insertData) {
                sc = new Scanner(new FileReader(tableInFile));
                Table.tables.clear();
                root = Table.getTable("clinicalRoot");
                wkTable = null;
                wkColumn = null;
                while (sc.hasNextLine()) {
                    switch (sc.nextLine()) {
                        case "TABLE":
                            wkTable = Table.getTable(sc.nextLine());
                            wkColumn = null;
                            break;
                        case "COLUMN":
                            wkColumn = wkTable.getColumn(sc.nextLine());
                            break;
                        case "TYPE":
                            wkColumn.type = ColumnType.valueOf(sc.nextLine());
                            break;
                        case "MAXLENGTH":
                            wkColumn.maxLength = Integer.parseInt(sc.nextLine());
                            break;
                        case "MAXSCALE":
                            wkColumn.maxScale = Integer.parseInt(sc.nextLine());
                            break;
                        case "MAXPRECISION":
                            wkColumn.maxPrecision = Integer.parseInt(sc.nextLine());
                            break;
                        case "REFERENCE":
                            wkTable.references.add(Table.getTable(sc.nextLine()));
                            break;
                        default:
                            throw new Exception("Error parsing table file!");
                    }
                }
                sc.close();
                if (outputDetails) {
                    for (Table table : Table.tables.values()) {
                        System.out.println(table.toString());
                    }
                }
            }
            //</editor-fold>
            //<editor-fold desc="generate sql create scripts" defaultstate="collapsed">
            if (genTableScript) {
                out = new FileWriter(tableSqlFile, false);
                for (Table table : Table.tables.values()) {
                    out.write(table.getCreate());
                }
                for (Table table : Table.tables.values()) {
                    out.write(table.getForeignKeys());
                }
                out.write(Table.getDrop());
                out.close();
            }
            //</editor-fold>
            //<editor-fold desc="load data from xml file into the database" defaultstate="collapsed">
            if (insertData || updateDb){
                Class.forName("org.postgresql.Driver");
                con = DriverManager.getConnection("jdbc:postgresql://127.0.0.1:5432/GenomeProcessing", "username", "password");
            }
            if (updateDb){
                stmtQry = con.createStatement();
                stmtCmd = con.createStatement();
                rs = stmtQry.executeQuery("SELECT cos_id, cos_mutationgrch37genomeposition FROM tblcosmic WHERE cos_mutationgrch37genomeposition IS NOT NULL AND cos_mutationgrch37genomeposition <> ''");
                while (rs.next()){
                    int id = rs.getInt("cos_id");
                    String grch37 = rs.getString("cos_mutationgrch37genomeposition");
                    int chrom = Integer.parseInt(grch37.substring(0, grch37.indexOf(":")).trim());
                    int start = Integer.parseInt(grch37.substring(grch37.indexOf(":") + 1, grch37.indexOf("-")).trim());
                    int stop = Integer.parseInt(grch37.substring(grch37.indexOf("-") + 1).trim());
                    runSql("UPDATE tblcosmic SET"
                            + " cos_mutationgrch37genomepositionchromosome = '" + chrom + "'"
                            + ", cos_mutationgrch37genomepositionstart = '" + start + "'"
                            + ", cos_mutationgrch37genomepositionstop = '" + stop + "'"
                            + " WHERE cos_id = '" + id + "'", stmtCmd);
                }
            }
            if (insertData){
                sc = new Scanner(new FileReader(xmlInFile));
                //System.out.println(sc.nextLine());
                root = Table.getTable("clinicalRoot");
                TableRecord rootRecord = new TableRecord(root);
                extractDataFromXml(rootRecord, false);
                rootRecord.clear();
                sc.close();
            }
            //</editor-fold>
        } catch (Exception e) {
            if (wkSql != null){
                System.err.println(wkSql);
            }
            e.printStackTrace(System.err);
        } finally {
            try {sc.close();} catch (Exception dontCare){}
            try {out.close();} catch (Exception dontCare){}
            try {rs.close();}catch(Exception dontCare){}
            try {stmtQry.close();}catch(Exception dontCare){}
            try {stmtCmd.close();}catch(Exception dontCare){}
            try {con.close();}catch(Exception dontCare){}            
        }
    }
    
    private static boolean parseXml(Table parentTable) throws Exception{
        while (scanNextToken()) {
            switch (currToken.type) {
                case PROP_OPEN:
                    if (scanNextToken() && currToken.type == PROP_NAME) {
                        if (drivingTable != null && drivingTable.equals(currToken.getValue())){
                            if (limit > 0){
                                count++;
                                if (count > limit){
                                    return false;
                                }
                            }
                        }
                        Table childTable = Table.getTable(currToken.getValue());
                        parentTable.references.add(childTable);
                        if (!parseXml(childTable)){
                            return false;
                        }
                    } else {
                        throw new Exception("Error parsing xml!");
                    }
                    break;
                case ATTR_NAME:
                    wkColumn = parentTable.getColumn(currToken.getValue());
                    if (scanNextToken() && currToken.type == ATTR_EQ
                            && scanNextToken() && currToken.type == ATTR_VAL){

                        wkColumn.updateTypeAndSize(currToken.getValue());
                    } else {
                        throw new Exception("Error parsing xml!");
                    }
                    break;
                case PROP_CLOSE:
                    break;
                case CLOSE_TAG_OPEN:
                    if (!(scanNextToken() && currToken.type == CLOSE_TAG_NAME
                            && scanNextToken() && currToken.type == CLOSE_TAG_CLOSE)){
                        throw new Exception("Error parsing xml!");
                    }                    
                    return true;
                case PROP_CLOSE_TAG:
                    return true;
                case PROP_VAL:
                    wkColumn = parentTable.getColumn("valueColumn");
                    wkColumn.updateTypeAndSize(currToken.getValue());
                    break;                    
                default:
                    throw new Exception("error poarsing xml!");
            }
        }
        return true;
    }

    private static boolean extractDataFromXml(TableRecord parentTableRecord,
            boolean trackParentData) throws Exception{
        while (scanNextToken()) {
            switch (currToken.type) {
                case PROP_OPEN:
                    if (scanNextToken() && currToken.type == PROP_NAME) {
                        TableRecord childTableRecord = new TableRecord(Table.getTable(currToken.getValue()));
                        if (trackParentData){
                            parentTableRecord.references.add(childTableRecord);
                        }
                        boolean trackChildData = trackParentData || childTableRecord.table.name.equals(drivingTable);
                        if (!extractDataFromXml(childTableRecord, trackChildData)){
                            return false;
                        }
                    } else {
                        throw new Exception("Error parsing xml!");
                    }
                    break;
                case ATTR_NAME:
                    wkColumn = parentTableRecord.table.getColumn(currToken.getValue());
                    wkString = parentTableRecord.table.getColumnName(wkColumn);
                    if (scanNextToken() && currToken.type == ATTR_EQ
                            && scanNextToken() && currToken.type == ATTR_VAL){
                        parentTableRecord.fields.put(wkString, (wkColumn.type.getValueForInsert(currToken.getValue())));
                    } else {
                        throw new Exception("Error parsing xml!");
                    }
                    break;
                case PROP_CLOSE:
                    break;
                case CLOSE_TAG_OPEN:
                    if (!(scanNextToken() && currToken.type == CLOSE_TAG_NAME
                            && scanNextToken() && currToken.type == CLOSE_TAG_CLOSE)){
                        throw new Exception("Error parsing xml!");
                    }
                case PROP_CLOSE_TAG:
                    if (drivingTable.equals(parentTableRecord.table.name)) {
                        runSql(parentTableRecord.getInsert());
                        parentTableRecord.clear();
                        if (limit > 0) {
                            count++;
                            if (count > limit) {
                                return false;
                            }
                        }
                    }
                    return true;
                case PROP_VAL:
                    wkColumn = parentTableRecord.table.getColumn("valueColumn");
                    wkString = parentTableRecord.table.getColumnName(wkColumn);
                    parentTableRecord.fields.put(wkString, wkColumn.type.getValueForInsert(currToken.getValue()));
                    break;                    
                default:
                    throw new Exception("error poarsing xml!");
            }
        }
        return false;
    }
    
    private static void runSql(String sql) throws Exception{
        Statement stmt = con.createStatement();
        runSql(sql, stmt);
        stmt.close();
    }

    private static void runSql(String sql, Statement stmt) throws Exception{
        wkSql = sql;
        if(outputDetails){
            System.out.println(sql);
        }
        if (!test) {
//            if(++insertCount%1000==0){
//                System.out.println(insertCount);
//            }
            stmt.execute(sql);
        }
    }

    private static boolean scanNextToken() throws Exception{
        while (line == null || line.trim().isEmpty()){
            if (!sc.hasNextLine()){
                return false;
            } else {
                line = sc.nextLine().trim();
                if(++lineCount%1000==0){
                    System.out.println(lineCount);
                }
                if (line.startsWith("<?xml")){
                    line = "";
                    continue;
                }
            }
        }
        while (true){
            try {
                currToken = XmlToken.getNext(currToken, line);
                break;
            } catch(Exception e){
                if (currToken.type == PROP_CLOSE){
                    line += sc.nextLine().trim();
                    if(++lineCount%1000==0){
                        System.out.println(lineCount);
                    }
                    continue;
                } else {
                    throw e;
                }
            }
        }
        line = line.substring(currToken.anteparseValue.length()).trim();
        if (outputDetails){
            echoParse(currToken, line);
        }
        return true;
    }
    
    private static void echoParse(XmlToken token, String line){
        if (outputDetails) {
            System.out.println(token.toString());
            System.out.println("line=" + line);
            System.out.println();
        }
    }

    private static void echoParseAndWait(XmlToken token, String line) throws Exception{
        echoParse(token, line);
        System.in.read(inBuffer);
    }
}
