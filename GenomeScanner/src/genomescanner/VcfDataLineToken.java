/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package genomescanner;

import java.util.EnumMap;
import static genomescanner.VcfDataLineToken.VcfDataLineTokenType.*;

/**
 *
 * @author bill
 */
class VcfDataLineToken {

    public enum VcfDataLineTokenType {

        START(""),
        INFO_ID_SEP_SEMI("[;]"),
        INFO_VAL_SEP_COMMA("[,]"),
        FORMAT_VAL_SEP_COMMA("[,]"),
        ALT_VAL_SEP_COMMA("[,]"),
        INFO_EQUALS("[=]"),
        FORMAT_ID_SEP_COLON("[:]"),
        FORMAT_VAL_SEP_COLON("[:]"),
        INFO_VAL_STRING("[^:;,\\t ][^:;,\\t ]*"),
        FORMAT_VAL_STRING("[^:;,\\t ][^:;,\\t ]*"),
        INFO_START(""),
        INFO_LOOP(""),
        FORMAT_START(""),
        FORMAT_LOOP(""),
        CHROM("[0-9xyXY][0-9xyXY]*"),
        POS("[0-9][0-9]*"),
        ID("[^ \\t][^ \\t]*"),
        REF("[atcguATCGU][atcguATCGU]*"),
        ALT("[atcguATCGU][atcguATCGU]*"),
        QUAL("[0-9.][0-9.]*"),
        FILTER("[^ \\t][^ \\t]*"),
        INFO_DP("[dD][pP]"),
        INFO_DP4("[dD][pP][4]"),
        INFO_MQ("[mM][qQ]"),
        INFO_FQ("[fF][qQ]"),
        INFO_AF1("[aA][fF][1]"),
        INFO_AC1("[aA][cC][1]"),
        INFO_AN("[aA][nN]"),
        INFO_IS("[iI][sS]"),
        INFO_AC("[aA][cC]"),
        INFO_G3("[gG][3]"),
        INFO_HWE("[hH][wW][eE]"),
        INFO_CLR("[cC][lL][rR]"),
        INFO_UGT("[uU][gG][tT]"),
        INFO_CGT("[cC][gG][tT]"),
        INFO_PV4("[pP][vV][4]"),
        INFO_INDEL("[iI][nN][dD][eE][lL]"),
        INFO_PC2("[pP][cC][2]"),
        INFO_PCHI2("[pP][cC][hH][iI][2]"),
        INFO_QCHI2("[qQ][cC][hH][iI][2]"),
        INFO_PR("[pP][rR]"),
        INFO_QBD("[qQ][bB][dD]"),
        INFO_RPB("[rR][pP][bB]"),
        INFO_MDV("[mM][dD][vV]"),
        INFO_VDB("[vV][dD][bB]"),
        FORMAT_GT("[gG][tT]"),
        FORMAT_GQ("[gG][qQ]"),
        FORMAT_GL("[gG][lL]"),
        FORMAT_DP("[dD][pP]"),
        FORMAT_DV("[dD][vV]"),
        FORMAT_SP("[sS][pP]"),
        FORMAT_PL("[pP][lL]"),
        FORMAT_VALUES(""),
        END("");

        private String regex;
        private static final EnumMap<VcfDataLineTokenType, VcfDataLineTokenType[]> followers;

        static {
            followers = new EnumMap<>(VcfDataLineTokenType.class);
            followers.put(START, new VcfDataLineTokenType[]{CHROM});
            followers.put(CHROM, new VcfDataLineTokenType[]{POS});
            followers.put(POS, new VcfDataLineTokenType[]{ID});
            followers.put(ID, new VcfDataLineTokenType[]{REF});
            followers.put(REF, new VcfDataLineTokenType[]{ALT});
            followers.put(ALT, new VcfDataLineTokenType[]{QUAL, ALT_VAL_SEP_COMMA});
            followers.put(ALT_VAL_SEP_COMMA, new VcfDataLineTokenType[]{ALT});
            followers.put(QUAL, new VcfDataLineTokenType[]{FILTER});
            followers.put(FILTER, new VcfDataLineTokenType[]{INFO_START});
            followers.put(INFO_START, new VcfDataLineTokenType[]{INFO_DP4, INFO_DP,
                INFO_MQ, INFO_FQ, INFO_AF1, INFO_AC1, INFO_AN, INFO_IS, INFO_AC,
                INFO_G3, INFO_HWE, INFO_CLR, INFO_UGT, INFO_CGT, INFO_PV4,
                INFO_INDEL, INFO_PC2, INFO_PCHI2, INFO_QCHI2, INFO_PR, INFO_QBD,
                INFO_RPB, INFO_MDV, INFO_VDB, FORMAT_START
            });
            followers.put(INFO_LOOP, new VcfDataLineTokenType[]{INFO_DP4, INFO_DP,
                INFO_MQ, INFO_FQ, INFO_AF1, INFO_AC1, INFO_AN, INFO_IS, INFO_AC,
                INFO_G3, INFO_HWE, INFO_CLR, INFO_UGT, INFO_CGT, INFO_PV4,
                INFO_INDEL, INFO_PC2, INFO_PCHI2, INFO_QCHI2, INFO_PR, INFO_QBD,
                INFO_RPB, INFO_MDV, INFO_VDB
            });
            followers.put(INFO_DP, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_DP4, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_MQ, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_FQ, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_AF1, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_AC1, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_AN, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_IS, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_AC, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_G3, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_HWE, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_CLR, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_UGT, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_CGT, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_PV4, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_INDEL, new VcfDataLineTokenType[]{INFO_VAL_SEP_COMMA,
                INFO_ID_SEP_SEMI, FORMAT_START});
            followers.put(INFO_PC2, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_PCHI2, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_QCHI2, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_PR, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_QBD, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_RPB, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_MDV, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_VDB, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_VDB, new VcfDataLineTokenType[]{INFO_EQUALS});
            followers.put(INFO_EQUALS, new VcfDataLineTokenType[]{INFO_VAL_STRING});
            followers.put(INFO_VAL_STRING, new VcfDataLineTokenType[]{INFO_VAL_SEP_COMMA,
                INFO_ID_SEP_SEMI, FORMAT_START});
            followers.put(INFO_VAL_SEP_COMMA, new VcfDataLineTokenType[]{INFO_VAL_STRING});
            followers.put(INFO_ID_SEP_SEMI, new VcfDataLineTokenType[]{INFO_LOOP});
            followers.put(FORMAT_START, new VcfDataLineTokenType[]{FORMAT_GT,
                FORMAT_GQ, FORMAT_GL, FORMAT_DP, FORMAT_DV, FORMAT_SP,
                FORMAT_PL, END});
            followers.put(FORMAT_LOOP, new VcfDataLineTokenType[]{FORMAT_GT,
                FORMAT_GQ, FORMAT_GL, FORMAT_DP, FORMAT_DV, FORMAT_SP,
                FORMAT_PL});
            followers.put(FORMAT_GT, new VcfDataLineTokenType[]{FORMAT_ID_SEP_COLON, FORMAT_VALUES});
            followers.put(FORMAT_GQ, new VcfDataLineTokenType[]{FORMAT_ID_SEP_COLON, FORMAT_VALUES});
            followers.put(FORMAT_GL, new VcfDataLineTokenType[]{FORMAT_ID_SEP_COLON, FORMAT_VALUES});
            followers.put(FORMAT_DP, new VcfDataLineTokenType[]{FORMAT_ID_SEP_COLON, FORMAT_VALUES});
            followers.put(FORMAT_DV, new VcfDataLineTokenType[]{FORMAT_ID_SEP_COLON, FORMAT_VALUES});
            followers.put(FORMAT_SP, new VcfDataLineTokenType[]{FORMAT_ID_SEP_COLON, FORMAT_VALUES});
            followers.put(FORMAT_PL, new VcfDataLineTokenType[]{FORMAT_ID_SEP_COLON, FORMAT_VALUES});
            followers.put(FORMAT_ID_SEP_COLON, new VcfDataLineTokenType[]{FORMAT_LOOP});
            followers.put(FORMAT_VALUES, new VcfDataLineTokenType[]{FORMAT_VAL_STRING});
            followers.put(FORMAT_VAL_STRING, new VcfDataLineTokenType[]{FORMAT_VAL_SEP_COMMA, FORMAT_VAL_SEP_COLON, END});
            followers.put(FORMAT_VAL_SEP_COMMA, new VcfDataLineTokenType[]{FORMAT_VAL_STRING});
            followers.put(FORMAT_VAL_SEP_COLON, new VcfDataLineTokenType[]{FORMAT_VAL_STRING});
            followers.put(END, new VcfDataLineTokenType[]{FORMAT_VAL_STRING});

            for (VcfDataLineTokenType[] followerAry : followers.values()) {
                for (int i1 = 0; i1 < followerAry.length; i1++) {
                    for (int i2 = i1 + 1; i2 < followerAry.length; i2++) {
                        if (followerAry[i2].regex.startsWith(followerAry[i1].regex)
                                || followerAry[i2].name().startsWith(followerAry[i1].name())) {
                            VcfDataLineTokenType t = followerAry[i1];
                            followerAry[i1] = followerAry[i2];
                            followerAry[i2] = t;
                        }
                    }
                }
            }
        }

        VcfDataLineTokenType(String regex) {
            this.regex = "[ \\t]*" + regex + "[ \\t]*";
        }

        VcfDataLineTokenType[] getFollowers() throws Exception {
            return followers.get(this);
        }

        VcfDataLineTokenType getNextType(String line) throws Exception {
            line = line.trim();
            for (VcfDataLineTokenType follower : getFollowers()) {
                if (line.matches(follower.regex + ".*")) {
                    return follower;
                }
            }
            throw GenomeScannerException.create(GenomeScanner.lineCount, line, GenomeScanner.curFile, this);
        }

        String getValue(String line) throws Exception {
            line = line.trim();
            int length = line.length();
            switch (this) {
                default:
                    length -= line.replaceFirst(regex, "").length();
                    return line.substring(0, length);
            }
        }
    }

    public final VcfDataLineTokenType type;
    public final String anteparseValue;
    private static String wkString;

    private VcfDataLineToken(VcfDataLineTokenType type, String anteparseValue) {
        this.type = type;
        this.anteparseValue = anteparseValue;
    }

    public String getValue() {
        switch (type) {
            default:
                return anteparseValue.trim();
        }
    }

    private VcfDataLineToken getNext(String line) throws Exception {
        line = line.trim();
        VcfDataLineTokenType nextType = type.getNextType(line);
        String nextValue = nextType.getValue(line);
        return new VcfDataLineToken(nextType, nextValue);
    }

    public static VcfDataLineToken getNext(VcfDataLineToken prev, String line) throws Exception {
        line = line.trim();
        if (prev == null) {
            return new VcfDataLineToken(START, "");
        } else {
            return prev.getNext(line);
        }
    }

    @Override
    public String toString() {
        return this.getClass().getName() + ":"
                + " type=" + type.name()
                + ", anteparseValue=" + anteparseValue
                + ", value=" + getValue();
    }
}
