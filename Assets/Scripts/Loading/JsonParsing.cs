using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;
using System.Text;
using UnityEngine;

public static class JsonParsing
{
    // 역직렬화 함수
    public static object Deserialize(string json)
    {
        if(json == null)
        {
            return null;
        }

        return Parser.Parse(json);
    }

    // 파싱용 클래스
    sealed class Parser : IDisposable
    {
        const string WORD_BREAK = "{}[],:\"";

        public static bool IsWordBreak(char c)
        {
            return Char.IsWhiteSpace(c) || WORD_BREAK.IndexOf(c) != -1;
        }

        // 토큰 값에 대한 열거형(해당 토큰 값은 실제 값들이 어떠한 타입인지를 정할떄 사용)
        enum TOKEN
        {
            NONE,
            CURLY_OPEN,
            CURLY_CLOSE,
            SQUARED_OPEN,
            SQUARED_CLOSE,
            CLOLON,
            COMMA,
            STRING,
            NUMBER,
            TRUE,
            FALSE,
            NULL
        };

        StringReader json;

        // 초기화 함수
        Parser(string jsonString)
        {
            json = new StringReader(jsonString);
        }

        //문자열에서 값을 파싱한다.
        public static object Parse(string jsonString)
        {
            using (var instance = new Parser(jsonString))
            {
                return instance.ParseValue();
            }
        }

        public static object Parse(string str, object Type)
        {
            using (var instance = new Parser(str))
            {
                return instance.ParseValue(Type, str);
            }
        }
        public void Dispose()
        {
            json.Dispose();
            json = null;
        }

        Dictionary<string, object> ParseObject()
        {
            Dictionary<string, object> Table = new Dictionary<string, object>();

            json.Read();

            while(true)
            {
                switch(NextToken)
                {
                case TOKEN.NONE:
                        return null;
                    case TOKEN.COMMA:
                        continue;
                    case TOKEN.CURLY_CLOSE:
                        return Table;
                    default:
                        string name = ParseString();
                        if(name == null)
                        {
                            return null;
                        }

                        if(NextToken != TOKEN.CLOLON)
                        {
                            return null;
                        }

                        json.Read();

                        Table[name] = ParseValue();
                        break;
                }
            }
        }
        // 리스트 형태로 값들을 파싱해 오는 함수.
        List<object> ParseArray()
        {
            List<object> array = new List<object>();

            json.Read();

            var parsing = true;
            while(parsing)
            {
                TOKEN nextToken = NextToken;

                switch(nextToken)
                {
                    // 토큰값이 없으면 다 돈 것이므로 널 반환 하고 종료
                    case TOKEN.NONE:
                        return null;
                    // 토큰 값이 콤마이면 컨티뉴로 계속 진행
                    case TOKEN.COMMA:
                        continue;
                    case TOKEN.SQUARED_CLOSE:
                        parsing = false;
                        break;
                            // 그 외의 경우에는 다음값을 파싱해온다.
                    default:
                        object value = ParseByToken(nextToken);
                        array.Add(value);
                        break;
                }
            }

            return array;
        }

        // 다은 데이터의 타입에 따라 실제 값들을 파싱해온다
        object ParseValue()
        {
            TOKEN nextToken = NextToken;
            return ParseByToken(nextToken);
        }

        object ParseValue(object Type, string str)
        {
            FieldInfo[] infos = Type.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
            
            foreach (var item in infos)
            {
                if ((item.GetValue(Type) as IDictionary) != null)
                {
                    //str.IndexOf()
                }
                else if ((item.GetValue(Type) as IList) != null)
                {
                    
                }
                else if ((item.GetValue(Type) as string) != null)
                {
                    
                }
                else if (item.GetValue(Type) is bool)
                {
                    
                }
                else if (item.GetValue(Type) is char)
                {
                    

                }
                else if (item.GetValue(Type) == null)
                {
                    
                }
                else
                {
                    
                }
            }

            return Type;
        }
        // 현재의 토큰 값에 따라 어떠한 Type의 데이터인지를 판단하고 해당 타입에 맞게 파싱해온다.
        object ParseByToken(TOKEN tOKEN)
        {
            switch(tOKEN)
            {
                case TOKEN.STRING:
                    return ParseString();
                case TOKEN.NUMBER:
                    return ParseNumber();
                case TOKEN.CURLY_OPEN:
                    return ParseObject();
                case TOKEN.SQUARED_OPEN:
                    return ParseArray();
                case TOKEN.TRUE:
                    return true;
                case TOKEN.FALSE:
                    return false;
                case TOKEN.NULL:
                    return null;
                default:
                    return null;
            }
        }

        // 문자열을 파싱해오는 함수
        string ParseString()
        {
            StringBuilder builder = new StringBuilder();
            char c;

            json.Read();

            bool parsing = true;

            while(parsing)
            {
                if(json.Peek() == -1)
                {
                    parsing = true;
                    break;
                }

                c = NextChar;
                switch(c)
                {
                    case '"':
                        parsing = false;
                        break;
                    case '\\':
                        if (json.Peek() == -1)
                        {
                            parsing = false;
                            break;
                        }

                        c = NextChar;
                        switch (c)
                        {
                            case '"':
                            case '\\':
                            case '/':
                                builder.Append(c);
                                break;
                            case 'b':
                                builder.Append('\b');
                                break;
                            case 'f':
                                builder.Append('\f');
                                break;
                            case 'n':
                                builder.Append('\n');
                                break;
                            case 'r':
                                builder.Append('\r');
                                break;
                            case 't':
                                builder.Append('\t');
                                break;
                            case 'u':
                                var hex = new char[4];

                                for (int i = 0; i < 4; i++)
                                {
                                    hex[i] = NextChar;
                                }

                                builder.Append((char)Convert.ToInt32(new string(hex), 16));
                                break;
                        }
                        break;
                    default:
                        builder.Append(c);
                        break;

                }
            }

            return builder.ToString();
        }

        // 숫자를 파싱 해 오는 함수
        object ParseNumber()
        {
            string number = NextWord;

            if(number.IndexOf('.') == -1)
            {
                long parsedint;
                Int64.TryParse(number, out parsedint);
                return parsedint;
            }

            double parserDouble;
            Double.TryParse(number, out parserDouble);
            return parserDouble;
        }

        void EatWhitespace()
        {
            while(Char.IsWhiteSpace(PeekChar))
            {
                json.Read();

                if(json.Peek() == -1)
                {
                    break;
                }
            }
        }

        char PeekChar
        {
            get { return Convert.ToChar(json.Peek()); }
        }

        char NextChar
        {
            get { return Convert.ToChar(json.Read()); }
        }

        string NextWord
        {
            get
            {
                StringBuilder word = new StringBuilder();

                while(!IsWordBreak(PeekChar))
                {
                    word.Append(NextChar);

                    if(json.Peek() == -1)
                    {
                        break;
                    }
                }

                return word.ToString();
            }
        }

        TOKEN NextToken
        {
            get
            {
                EatWhitespace();

                if(json.Peek() == -1)
                {
                    return TOKEN.NONE;
                }

                switch(PeekChar)
                {
                    case '{':
                        return TOKEN.CURLY_OPEN;
                    case '}':
                        json.Read();
                        return TOKEN.CURLY_CLOSE;
                    case '[':
                        return TOKEN.SQUARED_OPEN;
                    case ']':
                        json.Read();
                        return TOKEN.SQUARED_CLOSE;
                    case ',':
                        json.Read();
                        return TOKEN.COMMA;
                    case '"':
                        return TOKEN.STRING;
                    case ':':
                        return TOKEN.CLOLON;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '-':
                        return TOKEN.NUMBER;
                }

                switch(NextWord)
                {
                    case "false":
                        return TOKEN.FALSE;
                    case "true":
                        return TOKEN.TRUE;
                    case "null":
                        return TOKEN.NULL;
                }

                return TOKEN.NONE;
            }
        }
    }

    public static string Serialize(object obj)
    {
        return Serializer.Serialize(obj);
    }

    // 직렬화 클래스
    sealed class Serializer
    {
        StringBuilder builder;
        Serializer()
        {
            builder = new StringBuilder();
        }

        public static string Serialize(object obj)
        {
            var instance = new Serializer();
            instance.SerializeValue(obj);
            return instance.builder.ToString();
        }

        void SerializeValue(object value)
        {
            builder.Append("{");
            FieldInfo[] Infos = value.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

            IList asList;
            Array asArray;
            IDictionary asDict;
            string asStr;
            bool first = true;

            foreach (var item in Infos)
            {
                if (!first)
                {
                    builder.Append(',');
                }

                builder.Append("\"" + item.Name + "\":");

                if ((asDict = item.GetValue(value) as IDictionary) != null)
                {
                    SerializeObject(asDict);
                }
                else if ((asList = item.GetValue(value) as IList) != null)
                {
                    SerializeArray(asList);
                }
                else if ((asStr = item.GetValue(value) as string) != null)
                {
                    builder.Append(DataProcess.stringToNull(asStr));
                }
                else if (item.GetValue(value) is bool)
                {
                    builder.Append((bool)item.GetValue(value) ? "true" : "false");
                }
                else if(item.GetValue(value) is char)
                {
                    SerializeString(new string((char)item.GetValue(value), 1));

                }
                else if (item.GetValue(value) == null)
                {
                    builder.Append("null");
                }
                else
                {
                    SerializeOther(item.GetValue(value));
                }

                first = false;
            }

            builder.Append("}");
        }

        void SerializeObject(IDictionary obj)
        {
            bool first = true;

            builder.Append('{');

            foreach (object e in obj.Keys)
            {
                if (!first)
                {
                    builder.Append(',');
                }

                if (e != null)
                {
                    SerializeString(e.ToString());
                    builder.Append(':');

                    SerializeValue(obj[e]);
                }
                else
                {
                    builder.Append("null");
                }
                first = false;
            }

            builder.Append('}');
        }

        void SerializeArray(IList anArray)
        {
            builder.Append('[');

            bool first = true;

            foreach (object obj in anArray)
            {
                if (!first)
                {
                    builder.Append(',');
                }

                if (obj != null)
                {
                    SerializeValue(obj);
                }
                else
                {
                    builder.Append("null");   
                }

                first = false;
            }

            builder.Append(']');
        }

        void SerializeString(string str)
        {
            builder.Append('\"');

            char[] charArray = str.ToCharArray();
            foreach (var c in charArray)
            {
                switch (c)
                {
                    case '"':
                        builder.Append("\\\"");
                        break;
                    case '\\':
                        builder.Append("\\\\");
                        break;
                    case '\b':
                        builder.Append("\\b");
                        break;
                    case '\f':
                        builder.Append("\\f");
                        break;
                    case '\n':
                        builder.Append("\\n");
                        break;
                    case '\r':
                        builder.Append("\\r");
                        break;
                    case '\t':
                        builder.Append("\\t");
                        break;
                    default:
                        int codepoint = Convert.ToInt32(c);
                        if ((codepoint >= 32) && (codepoint <= 126))
                        {
                            builder.Append(c);
                        }
                        else
                        {
                            builder.Append("\\u");
                            builder.Append(codepoint.ToString("x4"));
                        }
                        break;
                }
            }

            builder.Append('\"');
        }

        void SerializeOther(object value)
        {
            
            if (value is float)
            {
                builder.Append(((float)value).ToString("R"));
            }
            else if (value is int
              || value is uint
              || value is long
              || value is sbyte
              || value is byte
              || value is short
              || value is ushort
              || value is ulong)
            {
                builder.Append(value);
            }
            else if (value is double
              || value is decimal)
            {
                builder.Append(Convert.ToDouble(value).ToString("R"));
            }
            else
            {
                SerializeString(value.ToString());
            }
        }
    }
}
