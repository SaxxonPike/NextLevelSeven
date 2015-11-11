using System;

namespace NextLevelSeven.Test.Testing
{
    public static class ExampleMessageRepository
    {
        public static readonly string A04 =
            @"MSH|^~\&|SENDER|DEV|RECEIVER|SYSTEM|20120731102052||ADT^A04|20731172052000155374|P|2.2|18
EVN|A04|20120731102052|||dtrn1
PID|||135792468||UPDATEDISCHN^OPTWO^R||19410115|M|||888 2ND AVE^^LOSANGELES^CA^90045||(310)564-6546|||M|AT|155374|949-84-9494
PV1||O|OQA|3|20||890|||MED||||1|||890|O||||||||||||||||||||||||||20101028|0500
PV2|||PAIN|||||20101028
GT1|UPDATEDISCHN^OPTWO^||||888 2ND AVE^^LOS ANGELES^CA^90045|(111)222-3333|||||S |949-84-9494|||||||
IN1|1|MO|||MEDICARE A   B|MEDICARE A   B|MUTUAL OF OMAHA^BOX 1602-MEDICARE  DEPT^OMAHA^NEBRASKA^92668|123456789|(123)4567890||||||||^^||MO||||||0.00|0||||010101|dtr|||||||1|1||""||""||||Y|";

        public static readonly string BadSubComponent =
            @"MSH|^~\&|SENDER|DEV|RECEIVER|SYSTEM|201310091028||ORU^R01|1424734.1|P|2.3
PID|1|SSD0201941|SA0062046||1^&&&1J1234567&&&99SFC^^201407152100^201407171142^RTN||19231214|M||C|^Some Rd.^Somewhere^IL^68763|||||M||SA0003176321|000-00-0000
OBR|1|542354^PCM|5432543^LA01|1048^CULTURE WOUND^LA01^1048^CULTURE WOUND^LA01|1||201409081255|||305518||||201409081506|144&Wound^^^^^CON&CONTAINER|74423^FRASER*^NIKKAYA^M|(260)434-6214^9,4346496&&&&&&&&NOTE^NOTE^&&&&&&&&NOTE|||||201409111054||MIC|F||1^^^201409081255^^1|74423^FRASER*^NIKKAYA^M|||||||||||^^^^1";

        public static readonly string Minimum = @"MSH|^~\&";

        public static readonly string MshOnly =
            @"MSH|^~\&|SENDER|DEV|RECEIVER|SYSTEM|201310091028||ORU^R01|1424734.1|P|2.3";

        public static readonly string MultipleMessagesAsMlp =
            String.Format("{0}{1}1{2}{0}{1}2{2}{0}{1}3{2}", "\xB", "MSH|^~\\&|", "\x1C\xD");

        public static readonly string MultipleMessagesWithMultipleLinesAsMlp =
            String.Format("{0}{1}1{2}{0}{1}2{2}{0}{1}3{2}", "\xB", "MSH|^~\\&|\xDPID|1234", "\x1C\xD");

        public static readonly string MultipleMessagesSeparatedByLines =
            String.Format("{0}1{1}{1}{1}{1}{0}2{1}{1}{0}3", "MSH|^~\\&|", "\r\n");

        public static readonly string MultipleObr =
            @"MSH|^~\&|SENDER|DEV|RECEIVER|SYSTEM|201003150118||ORU^R01|5101|P|2.3|||NE|NE|
PID|1|0034157|002993817||LASTNAME^FIRSTNAME||19520101|M|||1234 MAIN^^DEARBORN HEIGHT^MI^48127||||||||
PV1|1|R|||||||||||||||||
OBR|1|24603|24603|PROTIME^PROTIME PANEL^L||201003150029|201003150029|||RA^ER||||201003150029|^^|OLS^OLSTAD^^^^^^L||||||201003150118|||F||^^^^^R||||^^I9
OBX|1|NM|PT^PROTIME^L||24.4|SEC|14.0-16.0|H|||F|||||RA
OBX|2|NM|INR^INR^L||2.1|RATIO|2.0-3.0|N|||F|||||RA
OBR|2|24603|24603|CBC^COMPLETE BLOOD COUNT^L||201003150029|201003150029|||RA^ER||||201003150029|^^|OLS^OLSTAD^^^^^^L||||||201003150118|||F||^^^^^R||||^^I9
OBX|1|NM|WBC^WBC^L||5.5|x10\S\3/uL|4.1-10.9|N|||F|||||RA
OBX|2|NM|RBC^RBC^L||3.83|x10\S\6/uL|4.20-6.30|L|||F|||||RA
OBX|3|NM|HGB^HGB^L||12.0|g/dL|12.0-18.0|N|||F|||||RA
OBX|4|NM|HCT^HCT^L||35|%|37-51|L|||F|||||RA
OBX|5|NM|MCV^MCV^L||91|fL|80-97|N|||F|||||RA
OBX|6|NM|MCHC^MCHC^L||35|g/dL|31-36|N|||F|||||RA
OBX|7|NM|RDW^RDW^L||12.9|%|11.4-14.5|N|||F|||||RA
OBX|8|NM|PLT^PLT^L||154|x10\S\3/uL|140-440|N|||F|||||RA
OBX|9|NM|LY%^LYMPH%^L||21|%|20-45|N|||F|||||RA
OBX|10|NM|MCH^MCH^L||31|pg|26-32|N|||F|||||RA
OBX|11|NM|MPV^MPV^L||8|fL|||||F|||||RA
OBX|12|NM|BA%^BASO%^L||2.3|%|||||F|||||RA
OBR|3|24603|24603|CMP^COMPREHENSIVE METABOLIC PROF^L||201003150029|201003150029|||RA^ER||||201003150029|^^|OLS^OLSTAD^^^^^^L||||||201003150118|||F||^^^^^R||||^^I9
OBX|1|NM|GLU^GLUCOSE^L||127|mg/dL|74-110|H|||F|||||RA
OBX|2|NM|BUN^BUN^L||22|mg/dL|7-25|N|||F|||||RA
OBX|3|NM|CR^CREATININE^L||2.3|mg/dL|0.6-1.3|H|||F|||||RA
OBX|4|NM|BN/CR^BUN/CREAT RATIO^L||9.6|CALC|12.0-20.0|L|||F|||||RA
OBX|5|NM|NA^SODIUM^L||144|mmol/L|136-145|N|||F|||||RA
OBX|6|NM|K^POTASSIUM^L||4.2|mmol/L|3.5-5.1|N|||F|||||RA
OBX|7|NM|CL^CHLORIDE^L||104|mmol/L|98-107|N|||F|||||RA
OBX|8|NM|CO2^CARBON DIOXIDE^L||28.7|mmol/L|21.0-32.0|N|||F|||||RA
OBX|9|NM|CA^CALCIUM^L||8.6|mg/dL|8.5-10.1|N|||F|||||RA
OBX|10|NM|TP^TOTAL PROTEIN^L||7.5|g/dL|6.7-8.5|N|||F|||||RA
OBX|11|NM|ALB^ALBUMIN^L||3.2|g/dL|3.4-5.0|L|||F|||||RA
OBX|12|NM|GLOB^GLOBULIN^L||4.3|CALC|2.6-3.9|H|||F|||||RA
OBX|13|NM|ALKP^ALK. PHOS.^L||138|U/L|50-136|H|||F|||||RA
OBX|14|NM|ALT^ALT (SGPT)^L||39|U/L|30-65|N|||F|||||RA
OBX|15|NM|AST^AST (SGOT)^L||27|U/L|15-37|N|||F|||||RA
OBX|16|NM|TBIL^TOTAL BILIRUBIN^L||0.63|mg/dL|0.00-1.00|N|||F|||||RA
OBR|4|24603|24603|LPBN^NT-PRO BNP^L||201003150029|201003150029|||RA^ER||||201003150029|^^|OLS^OLSTAD^^^^^^L||||||201003150118|||F||^^^^^R||||^^I9
OBX|1|NM|LPBN^NT-PRO BNP^L||2003.2|pg/ml|0.0-125.0|H|||F|||||RA";

        public static readonly string MultiplePid =
            @"MSH|^~\&|SENDER|DEV|RECEIVER|SYSTEM|2007100914484648||ORU^R01|0000111122223333444|P|2.3|
PID|1|0034157|002993817||LASTNAME^FIRSTNAME||19520101|M|||1234 MAIN^^DEARBORN HEIGHT^MI^48127||||||||
PID|1||94000000000^^^Priority Health||LASTNAME^FIRSTNAME||19400101|F|
PD1|1|||1234567890^DOCLAST^DOCFIRST^M^^^^^NPI|
OBR|1|||80061^LIPID PROFILE^CPT-4||20070911||||||||||
OBX|1|NM|13457-7^LDL (CALCULATED)^LOINC|49.000|MG/DL| 0.000 - 100.000|N|||F|
OBX|2|NM|2093-3^CHOLESTEROL^LOINC|138.000|MG/DL|100.000 - 200.000|N|||F|
OBX|3|NM|2086-7^HDL^LOINC|24.000|MG/DL|45.000 - 150.000|L|||F|
OBX|4|NM|2571-8^TRIGLYCERIDES^LOINC|324.000|MG/DL| 0.000 - 150.000|H|||F|";

        public static readonly string RepeatingName = @"MSH|^~\&|SENDER|DEV|RECEIVER|SYSTEM|20140522103041||ADT^A60||P||
PID|1|SSD0201941|SA0062046||Lincoln^Abe~Bro~Dude||19231214|M||C|^Some Rd.^Somewhere^IL^68763|||||M||SA0003176321|000-00-0000
PV1|||SA.OPS";

        public static readonly string Standard =
            @"MSH|^~\&|SENDER|DEV|RECEIVER|SYSTEM|20130528073829||ADT^A17|14150278|P|2.3|
EVN|A17|20130528073829|||ID659^Colon^Gabriel|20130528073829
PID|||||Colon^Ada^^^^||19930501|F||B|Claire Cv^^Lacona^IA^50139^^|LOS |^PRN^PH^^^^^^HOME PHONE||ENG|U|UNK|||||N|||||||||
PV1||I|1S^1S46^D|1||1S^1S59^A|ID419^Colon^Gabriel^Y^^MD|||CRIS||||E|||ID760^Velez^Gabriel^Y^^MD|MED|15318361|20|||||||||N||||||||||||A|||||||||
PV2||SP4|PRELIM: PSYCH SYMPTOMS|||||||||||||||||||||A||20130523|||||||||||
OBX|1|NM|1010.3^Height^AS4^3137-7^Height^LN|1|165.1|cm^""^""^""^""^""|||||P|||||
OBX|2|NM|1010.1^Weight^AS4^8335-2^Weight^LN|1|54.432|kg^""^""^""^""^""|||||P|||||
PID|||||David^Janelle^^^^|Hawkins|19810727|M||B|Cummings Rd^^Norwood^CO^43837^^|LOS |^PRN^PH^^^^^^HOME PHONE~^ORN^CP||ENG|U|PRO|||||N|||||||||
PV1||I|1S^1S59^A|9||1S^1S46^D|ID304^Morales^Gabriel^Y^^MD|||PSI||||9|||ID684^Noel^Gabriel^Y^^MD|MED|15321524|50|||||||||N||||||||||||A|||||||||
PV2||P|PRELIM:PSYCHOTIC DISORDER|||||||||||||||||||||A||20130524|||||||||||
OBX|1|NM|1010.3^Height^AS4^3137-7^Height^LN|1|165.1|cm^""^""^""^""^""|||||P|||||
Z10|||MED|1^0^00^00
ZCQ|1|1|CE|HOME^HOMELESS?|280005055^NH^NOT HOMELESS|||";

        public static readonly string Variety = @"MSH|^~\&|a&b^c&d~e&f^g&h~q&r^s&t|i&j^k&l~m&n^o&p~u&v^w&x";

        public static readonly string VersionlessMessage =
            @"MSH|^~\&|SENDER|DEV|RECEIVER|SYSTEM|20140522103041||ADT^A60||P||
PID|1|SSD0201941|SA0062046||Lincoln^Abe||19231214|M||C|^Some Rd.^Somewhere^IL^68763|||||M||SA0003176321|000-00-0000
PV1|||SA.OPS";
    }
}