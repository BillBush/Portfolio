/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package genomescanner;

import java.io.FileWriter;
import java.util.ArrayList;

/**
 *
 * @author bill
 */
class Column{
    public String group;
    public String id;
    public String desc;
    public String type;
    public String number;
    public static ArrayList<Column> columns = new ArrayList<>();
    
    public Column(){
    }
        
    public static void add(Column column) throws GenomeScannerException{
        int i = 0;
        for(; i < columns.size(); i++){
            Column col = columns.get(i);
            if (col.group.equals(column.group) && col.id.equals(column.id)){
                if(!col.desc.equals(column.desc)
                        || !col.type.equals(column.type)
                        || !col.number.equals(column.number)){
                    throw new GenomeScannerException(
                            "Variation in column definitions:\n"
                                    + col.toString() + "\n"
                                    + column.toString() + "\n");
                }
            }
        }
        if (i == columns.size()){
            columns.add(column);
        }
    }
    
    public String getColumnName(){
        return group + "_" + id + "_" + desc.replaceAll("[\"]", "").replaceAll("[(].*", "").replaceAll(" ", "").replaceAll("#", "num");
    }
    
    public String getColumnType() throws GenomeScannerException{
        if (type.matches("([iI][nN][tT][eE][gG][eE][rR]|[fF][lL][aA][gG])")){
            return "INTEGER";
        } else if(type.matches("[fF][lL][oO][aA][tT]")){
            return "DOUBLE";
        } else if(type.matches("[sS][tT][rR][iI][nN][gG]")){
            return "VARCHAR(250)";
        } else {
            throw new GenomeScannerException("Invalid column type: " + type);
        }
    }
    
    public String toString(){
        return "##"+group+"=<ID="+id+",Number="+number+",Type="+type+",Description="+desc+">";
    }
    
    public static void generateSql(String outFile) throws Exception{
        FileWriter fw = null;
        try{
            fw = new FileWriter(outFile, false);
            for(Column col : columns){
                if (!col.number.equals("1")){
                    continue;
                }
                fw.write("\t" + col.getColumnName() + " " + col.getColumnType() + ",\n");
            }
            for(Column col : columns){
                if (col.number.equals("1")){
                    continue;
                }
                fw.write(col.toString() + "\n");
            }
            fw.close();
        }catch (Exception e){
            throw e;
        } finally {
            try { fw.close(); } catch (Exception dontcare){}
        }
    }
    
    public static void generateRegex(String outFile) throws Exception{
        FileWriter fw = null;
        try{
            StringBuilder sb = null;
            fw = new FileWriter(outFile, false);
            for(Column col : columns){
                sb = new StringBuilder();
                sb.append(col.group.toUpperCase());
                sb.append("_");
                sb.append(col.id.toUpperCase());
                sb.append("(\"");
                for(char c : col.id.toCharArray()){
                    sb.append("[");
                    if (Character.isDigit(c)){
                        sb.append(c);
                    } else {
                        sb.append(Character.toLowerCase(c));
                        sb.append(Character.toUpperCase(c));
                    }
                    sb.append("]");
                }
                sb.append("\"),\n");
                fw.write(sb.toString());
                System.out.print(sb.toString());
            }
            fw.close();
        }catch (Exception e){
            throw e;
        } finally {
            try { fw.close(); } catch (Exception dontcare){}
        }
    }
}

