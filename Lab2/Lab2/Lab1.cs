using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class Lab1
    {
        enum Token
        {
            KeyWord,//关键字
            Identifier,//标识符
            Operation,//操作符
            Delimiter,//分隔符
            Int,//整数
            Double,//浮点数
            Annotation//注释符
        }
        enum State
        {
            Normal,//正常情况
            Var,//标识符
            Add,//加
            Minus,//减
            Less,//小于
            More,//大于
            Equal,//等于
            Exclamation,//惊叹号
            And,//且
            Or, //或
            Slash,//斜线
            Digit,//数字
            Decimal,//小数
            Annotation, //一行注释
            Annotations, //多行注释
            Annotation_star,//多行注释中的*
            Singlequotation,//单引号
            Doublequotation//双引号
        }
        private const string File_name = "test.txt";
        string word = "";
        List<string> list = new List<string>();
        string r = "";
        BinaryReader br;
        State state;
        int line = 1;
        string[] key = { "int", "float", "long", "char", "double", "enum", "void", "if", "else", "switch", "case", "break", "default", "for", "continue", "do", "while", "foreach", "in" };
        
        public string start(string s)
        {
            if (File.Exists(File_name))
            {
                FileStream fs = new FileStream(File_name, FileMode.Open, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(s);
                sw.Close();
            }
            if (File.Exists(File_name))
            {
                FileStream fs = new FileStream(File_name, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
            }
            Analysis();
            br.Close();
            if (File.Exists("result.txt"))
            {
                FileStream fs = new FileStream("result.txt", FileMode.Open, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs); // 创建写入流
                for (int i = 0; i < list.Count; i++)
                {
                    sw.WriteLine(list[i]);
                }
                sw.Close(); //关闭文件
            }
            return r;
        }

        public void Analysis()
        {
            state = State.Normal;

            try
            {
                while (true)
                {
                    char nextchar = getchar();
                    if (nextchar == (char)0)
                    {
                        break;
                    }
                    switch (state)
                    {
                        case State.Normal:
                            read(nextchar);
                            break;
                        case State.Var:
                            if ((nextchar >= 'a' && nextchar <= 'z') || (nextchar >= 'A' && nextchar <= 'Z') || (nextchar >= '0' && nextchar <= '9'))
                            {
                                word = word + nextchar.ToString();
                            }
                            else
                            {
                                if (isKey(word))
                                {
                                    list.Add("关键字，"+word);
                                    r = r + "k";
                                }
                                else
                                {
                                    list.Add("标识符"+word);
                                    r=r+"i";
                                }
                                state = State.Normal;
                                word = "";
                                read(nextchar);
                            }
                            break;
                        case State.Digit:
                            if (nextchar >= '0' && nextchar <= '9')
                            {
                                word = word + nextchar.ToString();
                            }
                            else if (nextchar == '.')
                            {
                                word = word + nextchar.ToString();
                                state = State.Decimal;
                            }
                            else if ((nextchar >= 'a' && nextchar <= 'z') || (nextchar >= 'A' && nextchar <= 'Z'))
                            {
                                error(line);
                                word = "";
                                nextchar = getchar();
                                while ((nextchar >= 'a' && nextchar <= 'z') || (nextchar >= 'A' && nextchar <= 'Z') || (nextchar >= '0' && nextchar <= '9'))
                                {
                                    nextchar = getchar();
                                }
                                state = State.Normal;
                                read(nextchar);
                            }
                            else
                            {
                                state = State.Normal;
                                list.Add("整数，" + word);
                                r = r + "d";
                                word = "";
                                read(nextchar);
                            }
                            break;
                        case State.Decimal:
                            if (nextchar >= '0' && nextchar <= '9')
                            {
                                word = word + nextchar.ToString();
                            }
                            else if ((nextchar >= 'a' && nextchar <= 'z') || (nextchar >= 'A' && nextchar <= 'Z'))
                            {
                                error(line);
                                word = "";
                                nextchar = getchar();
                                while ((nextchar >= 'a' && nextchar <= 'z') || (nextchar >= 'A' && nextchar <= 'Z') || (nextchar >= '0' && nextchar <= '9'))
                                {
                                    nextchar = getchar();
                                }
                                state = State.Normal;
                                read(nextchar);
                            }
                            else
                            {
                                state = State.Normal;
                                list.Add("浮点数，" + word);
                                r = r + "d";
                                word = "";
                                read(nextchar);
                            }
                            break;
                        case State.Add:
                            if (nextchar == '+' || nextchar == '=')
                            {
                                list.Add("操作符，" + "+" + nextchar.ToString());
                                r = r + "+" + nextchar.ToString();
                                state = State.Normal;
                            }
                            else
                            {
                                list.Add("操作符，" + "+");
                                r = r + "+";
                                state = State.Normal;
                                read(nextchar);
                            }
                            break;
                        case State.Minus:
                            if (nextchar == '-' || nextchar == '=')
                            {
                                list.Add("操作符，" + "-" + nextchar.ToString());
                                r = r + "-" + nextchar.ToString();
                                state = State.Normal;
                            }
                            else
                            {
                                list.Add("操作符，" + "-");
                                r = r + "-";
                                state = State.Normal;
                                read(nextchar);
                            }
                            break;
                        case State.Less:
                            if (nextchar == '<' || nextchar == '=')
                            {
                                list.Add("操作符，" + "<" + nextchar.ToString());
                                r = r + "<" + nextchar.ToString();
                                state = State.Normal;
                            }
                            else
                            {
                                list.Add("操作符，" + "<");
                                r = r + "<";
                                state = State.Normal;
                                read(nextchar);
                            }
                            break;
                        case State.More:
                            if (nextchar == '>' || nextchar == '=')
                            {
                                list.Add("操作符，" + ">" + nextchar.ToString());
                                r = r + ">" + nextchar.ToString();
                                state = State.Normal;
                            }
                            else
                            {
                                list.Add("操作符，" + ">");
                                r = r + ">";
                                state = State.Normal;
                                read(nextchar);
                            }
                            break;
                        case State.Equal:
                            if (nextchar == '=')
                            {
                                list.Add("操作符，" + "==");
                                r = r + "==";
                                state = State.Normal;
                            }
                            else
                            {
                                list.Add("操作符，" + "=");
                                r = r + "=";
                                state = State.Normal;
                                read(nextchar);
                            }
                            break;
                        case State.And:
                            if (nextchar == '&')
                            {
                                list.Add("操作符，" + "&&");
                                r = r + "&&";
                                state = State.Normal;
                            }
                            else
                            {
                                list.Add("操作符，" + "&");
                                r = r + "&";
                                state = State.Normal;
                                read(nextchar);
                            }
                            break;
                        case State.Or:
                            if (nextchar == '|')
                            {
                                list.Add("操作符，" + "||");
                                r = r + "||";
                                state = State.Normal;
                            }
                            else
                            {
                                list.Add("操作符，" + "|");
                                r = r + "|";
                                state = State.Normal;
                                read(nextchar);
                            }
                            break;
                        case State.Exclamation:
                            if (nextchar == '=')
                            {
                                list.Add("操作符，" + "!=");
                                r = r + "!=";
                                state = State.Normal;
                            }
                            else
                            {
                                list.Add("操作符，" + "!");
                                r = r + "!";
                                state = State.Normal;
                                read(nextchar);
                            }
                            break;
                        case State.Slash:
                            if (nextchar == '/')
                            {
                                list.Add("注释符，" + "//");
                                r = r + "//";
                                state = State.Annotation;
                            }
                            else if (nextchar == '*')
                            {
                                list.Add("注释符，" + "/*");
                                r = r + "/*";
                                state = State.Annotations;
                            }
                            else
                            {
                                list.Add("操作符，" + "/");
                                r = r + "/";
                                state = State.Normal;
                                read(nextchar);
                            }
                            break;
                        case State.Annotation:
                            if (nextchar == '\n')
                            {
                                line = line + 1;
                                list.Add("注释内容，" + word);
                                r = r + "a";
                                state = State.Normal;
                                word = "";
                            }
                            else
                            {
                                word = word + nextchar.ToString();
                            }
                            break;
                        case State.Annotations:
                            if (nextchar == '*')
                            {
                                state = State.Annotation_star;
                            }
                            word = word + nextchar.ToString();
                            if (nextchar == '\n')
                            {
                                line = line + 1;
                            }
                            break;
                        case State.Annotation_star:
                            if (nextchar == '/')
                            {
                                list.Add("注释内容，" + word.Substring(0, word.Length - 1));
                                r = r + "a";
                                list.Add("注释符，*/");
                                r = r + "*/";
                                word = "";
                                state = State.Normal;
                            }
                            else if (nextchar == '*')
                            {
                                word = word + nextchar.ToString();
                            }
                            else
                            {
                                state = State.Annotations;
                                word = word + nextchar.ToString();
                                if (nextchar == '\n')
                                {
                                    line = line + 1;
                                }
                            }
                            break;
                        case State.Singlequotation:
                            if (nextchar == '\'')
                            {
                                list.Add("字符，" + word);
                                r = r + "w";
                                list.Add("单引号，'");
                                r = r + "'";
                                word = "";
                                state = State.Normal;
                            }
                            else
                            {
                                word = word + nextchar.ToString();
                                if (nextchar == '\n')
                                {
                                    line = line + 1;
                                }
                            }
                            break;
                        case State.Doublequotation:
                            if (nextchar == '"')
                            {
                                list.Add("字符串，" + word);
                                r = r + "s";
                                list.Add("双引号，\"");
                                r = r + "\"";
                                word = "";
                                state = State.Normal;
                            }
                            else
                            {
                                word = word + nextchar.ToString();
                                if (nextchar == '\n')
                                {
                                    line = line + 1;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (EndOfStreamException e)
            {

            }
        }

        public void read(char c)
        {
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
            {
                word = word + c.ToString();
                state = State.Var;
            }
            else if ((c >= '0' && c <= '9'))
            {
                word = word + c.ToString();
                state = State.Digit;
            }
            else
            {
                switch (c)
                {
                    case '/':
                        state = State.Slash;
                        break;
                    case '+':
                        state = State.Add;
                        break;
                    case '-':
                        state = State.Minus;
                        break;
                    case '=':
                        state = State.Equal;
                        break;
                    case '!':
                        state = State.Exclamation;
                        break;
                    case '>':
                        state = State.More;
                        break;
                    case '<':
                        state = State.Less;
                        break;
                    case '&':
                        state = State.And;
                        break;
                    case '|':
                        state = State.Or;
                        break;
                    case '\'':
                        state = State.Singlequotation;
                        break;
                    case '"':
                        state = State.Doublequotation;
                        break;
                    case '{':
                        list.Add("左花括号，{");
                        r = r + "{";
                        break;
                    case '}':
                        list.Add("右花括号，}");
                        r = r + "}";
                        break;
                    case ';':
                        list.Add("分号，;");
                        r = r + ";";
                        break;
                    case ':':
                        list.Add("冒号，:");
                        r = r + ":";
                        break;
                    case ',':
                        list.Add("逗号，,");
                        r = r + ",";
                        break;
                    case '.':
                        list.Add("点号，.");
                        r = r + ".";
                        break;
                    default:
                        if (c == '(' || c == ')' || c == '[' || c == ']' || c == '*' || c == '^' || c == '%')
                        {
                            list.Add("操作符，" + c);
                            r = r + c;
                        }
                        else if (c == ' ' || c == '\t' || c == '\r') { }
                        else if (c == '\n')
                        {
                            line = line + 1;
                        }
                        else
                        {
                            list.Add("不能识别的字符，" + (int)c);
                            r = r + "u";
                        }

                        break;
                }
            }
        }

        public bool isKey(string s)
        {
            for (int i = 0; i < key.Count(); i++)
            {
                if (s == key[i])
                {
                    return true;
                }
            }
            return false;
        }

        public char getchar()
        {
            try
            {
                char c = br.ReadChar();
                return c;
            }
            catch (EndOfStreamException e)
            {
                return (char)0;
            }
        }

        public void error(int l)
        {
            list.Add("第" + l.ToString() + "行" + "输入有错误！");
            r = r + "e";
        }
    }
}
