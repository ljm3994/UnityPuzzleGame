using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataProcess
{
    public static int stringToint(string str)
    {
         int iTemp = 0;
    
         if (str != null)
         {
             if (!int.TryParse(str, out iTemp))
             {
                 iTemp = 0;
             }
         }
    
         return iTemp;
    }

    public static List<int> stringToListint(string str, string Separator)
    {
         string[] Sepa = new string[] { Separator };
         string[] Temp = str.Split(Sepa, System.StringSplitOptions.RemoveEmptyEntries);
         List<int> ReturnTemp = new List<int>();
    
         for(int i = 0; i < Temp.Length; i++)
         {
             if(!string.IsNullOrWhiteSpace(Temp[i]))
             {
                if (Temp[i] != "NULL" && Temp[i] != "null")
                {
                    ReturnTemp.Add(stringToint(Temp[i]));
                }
             }
         }
    
         return ReturnTemp;
    }

    public static string stringToNull(string str)
    {
        if(string.IsNullOrEmpty(str))
        {
            return "-";
        }

        return str;
    }
    public static bool stringTobool(string str)
    {
        bool ReturnBool = false;
        int intTemp = 0;

        if (str != null)
        {
            if (int.TryParse(str, out intTemp))
            {
                ReturnBool = intTemp == 1 ? true : false;
            }
            else
            {
                if (!bool.TryParse(str, out ReturnBool))
                {
                    ReturnBool = false;
                }
            }
        }

        return ReturnBool;
    }

    public static float stringTofloat(string str)
    {
        float Temp = 0.0f;

        if(str != null)
        {
            if(!float.TryParse(str, out Temp))
            {
                Temp = 0.0f;
            }
        }

        return Temp;
    }

    public static object GetClassInit(string strFullName, string[] arr)
    {
        Type type = Type.GetType(strFullName);
        
        if (type != null)
        {
            return Activator.CreateInstance(type, arr);
        }

        foreach (var appdomain in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = appdomain.GetType(strFullName);
            if (type != null)
            {
                return Activator.CreateInstance(type, arr);
            }
        }

        return null;
    }

    public static int PreCode(char c)
    {
        switch (c)
        {
            case '[':
            case ']':
                return 0;
            case '+':
            case '-':
                return 1;
            case '*':
            case '/':
                return 2;
            default:
                return -1;
        }
    }

    public static string InFixToposFix(string str)
    {
        Stack<char> infixToPostfixStack = new Stack<char>();

        char[] ca = str.ToCharArray();

        char popChar;
        string t = "";
        for (int i = 0, count = ca.Length; i < count; i++)
        {
            switch (ca[i])
            {
                case '+':
                case '-':
                case '*':
                case '/':
                    do
                    {
                        if (infixToPostfixStack.Count == 0)
                        {
                            break;
                        }
                        popChar = infixToPostfixStack.Pop();
                        if (PreCode(popChar) >= PreCode(ca[i]))
                        {
                            t += " " + popChar;
                        }
                        else
                        {
                            infixToPostfixStack.Push(popChar);
                            break;
                        }
                    } while (true);

                    t += " ";
                    infixToPostfixStack.Push(ca[i]);
                    break;

                case '[':
                    infixToPostfixStack.Push(ca[i]);
                    break;

                case ']':
                    do
                    {
                        popChar = infixToPostfixStack.Pop();
                        if (popChar == '[')
                        {
                            break;
                        }
                        t += " " + popChar;
                    } while (true);
                    break;

                default:
                    t += ca[i];
                    break;
            }
        }

        for (int i = 0, count = infixToPostfixStack.Count; i < count; i++)
        {
            t += " " + infixToPostfixStack.Pop();
        }

        return t;
    }

    public static double PostFixProcess(string s)
    {
        Stack<double> stack = new Stack<double>();

        double value = 0;
        double op1, op2;

        string[] sa = s.Split('[');

        if (s.Length == 0)
        {
            throw new System.ArgumentException("Parameter cannot be null", "string");
        }
        else if (s.Length == 1)
        {
            value = Convert.ToDouble(sa[0]);
            return value;
        }

        for (int i = 0, count = sa.Length; i < count; i++)
        {
            op2 = stack.Pop();
            op1 = stack.Pop();

            switch (sa[i])
            {
                case "+": value = op1 + op2; break;
                case "-": value = op1 - op2; break;
                case "*": value = op1 * op2; break;
                case "/": value = op1 / op2; break;
            }

            stack.Push(value);
        }

        value = stack.Pop();

        return value;
    }
}
