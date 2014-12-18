/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package genomescanner;

import java.util.EnumMap;
import static genomescanner.ColumnMetaToken.ColumnMetaTokenType.*;

/**
 *
 * @author bill
 */
class ColumnMetaToken{
    public enum ColumnMetaTokenType {
        COLUMN_META("##"),
        GROUP("([iI][nN][fF][oO]|[fF][oO][rR][mM][aA][tT])"),
        EQUALS("="),
        OPEN("<"),
        ID("[iI][dD]"),
        VALUE("([a-zA-Z0-9][a-zA-Z0-9]*|[\"][^\"][^\"]*[\"])"),
        COMMA("[,]"),
        NUMBER("[nN][uU][mM][bB][eE][rR]"),
        TYPE("[tT][yY][pP][eE]"),
        DESCRIPTION("[dD][eE][sS][cC][rR][iI][pP][tT][iI][oO][nN]"),
        CLOSE(">"),
        VCF_COLUMN_LINE("#");
        
        private String regex;
        private static final EnumMap<ColumnMetaTokenType, ColumnMetaTokenType[]> followers;
        
        static{
            followers = new EnumMap<>(ColumnMetaTokenType.class);
            followers.put(COLUMN_META, new ColumnMetaTokenType[]{GROUP});
            followers.put(GROUP, new ColumnMetaTokenType[]{EQUALS});
            followers.put(EQUALS, new ColumnMetaTokenType[]{OPEN,VALUE});
            followers.put(OPEN, new ColumnMetaTokenType[]{ID});
            followers.put(ID, new ColumnMetaTokenType[]{EQUALS});
            followers.put(VALUE, new ColumnMetaTokenType[]{COMMA,CLOSE});
            followers.put(COMMA, new ColumnMetaTokenType[]{NUMBER,TYPE,DESCRIPTION});
            followers.put(NUMBER, new ColumnMetaTokenType[]{EQUALS});
            followers.put(TYPE, new ColumnMetaTokenType[]{EQUALS});
            followers.put(DESCRIPTION, new ColumnMetaTokenType[]{EQUALS});
            followers.put(CLOSE, new ColumnMetaTokenType[]{COLUMN_META, VCF_COLUMN_LINE});
        }
        
        ColumnMetaTokenType(String regex){
            this.regex = "[ ]*" + regex + "[ ]*";
        }
        
        ColumnMetaTokenType[] getFollowers() throws Exception{
            return followers.get(this);
        }
        
        ColumnMetaTokenType getNextType(String line) throws Exception{
            line = line.trim();
            if (!line.isEmpty()){
                for (ColumnMetaTokenType follower : getFollowers()){
                    if(line.matches(follower.regex + ".*")){
                        return follower;
                    }
                }
                throw new Exception("Invalid token following " + this + " at "+ GenomeScanner.lineCount + ": "+ line);
            }
            return null;
        }
        
        String getValue(String line) throws Exception{
            line = line.trim();
            int length = line.length();
            if (!line.isEmpty()){
                switch(this){
                    default:
                        length -= line.replaceFirst(regex, "").length();
                        return line.substring(0, length);
                }
            }
            throw new Exception("no value defined for token, " + name() + ", in " + line);
        }
    }
    
    public final ColumnMetaTokenType type;
    public final String anteparseValue;
    private static String wkString;
    
    private ColumnMetaToken(ColumnMetaTokenType type, String anteparseValue){
        this.type = type;
        this.anteparseValue = anteparseValue;
    }

    public String getValue() {
        switch (type){
            default:
                return anteparseValue.trim();
        }
    }
    
    private ColumnMetaToken getNext(String line) throws Exception{
        line = line.trim();
        if (!line.isEmpty()){
            ColumnMetaTokenType nextType = type.getNextType(line);
            String nextValue = nextType.getValue(line);
            return new ColumnMetaToken(nextType, nextValue);
        }
        return null;
    }
    
    public static ColumnMetaToken getNext(ColumnMetaToken prev, String line) throws Exception {
        line = line.trim();
        if (!line.isEmpty()){
            if (prev == null){
                return new ColumnMetaToken(COLUMN_META, COLUMN_META.getValue(line));
            } else {
                return prev.getNext(line);
            }
        }
        return null;
    }
    
    @Override
    public String toString() {
        return this.getClass().getName() + ":"
                + " type=" + type.name()
                + ", anteparseValue=" + anteparseValue
                + ", value=" + getValue();
    }
}

