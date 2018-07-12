﻿using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xylang
{
    partial class XyLangVisitor
    {
        //public override object VisitTypeProtocol([NotNull] XyParser.TypeProtocolContext context)
        //{
        //    var obj = (string)Visit(context.typePackage());
        //    return obj;
        //}

        public override object VisitTypeTuple([NotNull] XyParser.TypeTupleContext context)
        {
            var obj = "";
            obj += "(";
            for(int i = 0; i < context.type().Length; i++)
            {
                if(i == 0)
                {
                    obj += Visit(context.type(i));
                }
                else
                {
                    obj += "," + Visit(context.type(i));
                }
            }
            obj += ")";
            return obj;
        }

        public override object VisitGetType([NotNull] XyParser.GetTypeContext context)
        {
            var r = new Result();
            r.data = "System.Type";
            r.text = "typeof(" + Visit(context.type()) + ")";
            return r;
        }

        public override object VisitTypeArray([NotNull] XyParser.TypeArrayContext context)
        {
            var obj = "";
            obj += " List<" + Visit(context.type()) + "> ";
            return obj;
        }

        public override object VisitTypeSharpArray([NotNull] XyParser.TypeSharpArrayContext context)
        {
            var obj = "";
            obj += Visit(context.type()) + "[]";
            return obj;
        }

        public override object VisitTypeDictinary([NotNull] XyParser.TypeDictinaryContext context)
        {
            var obj = "";
            obj += " Dictionary<" + Visit(context.type(0)) + "," + Visit(context.type(1)) + "> ";
            return obj;
        }

        public override object VisitTypePackage([NotNull] XyParser.TypePackageContext context)
        {
            var obj = "";
            obj += Visit(context.nameSpaceItem());
            if(context.templateCall() != null)
            {
                obj += Visit(context.templateCall());
            }
            return obj;
        }

        public override object VisitTypeFunction([NotNull] XyParser.TypeFunctionContext context)
        {
            var obj = "";
            var @in = (string)Visit(context.typeFunctionParameterClause(0));
            var @out = (string)Visit(context.typeFunctionParameterClause(1));
            if(@out.Length == 0)
            {
                if(@in.Length == 0)
                {
                    obj += "Action";
                }
                else
                {
                    obj += "Action<";
                    obj += @in;
                    obj += ">";
                }
            }
            else
            {
                if(@out.IndexOf(",") >= 0)
                {
                    @out = "(" + @out + ")";
                }
                if(@in.Length == 0)
                {
                    obj += "Func<";
                    obj += @out;
                    obj += ">";
                }
                else
                {
                    obj += "Func<";
                    obj += @in + ", ";
                    obj += @out;
                    obj += ">";
                }
            }
            return obj;
        }

        public override object VisitTypeFunctionParameterClause([NotNull] XyParser.TypeFunctionParameterClauseContext context)
        {
            var obj = "";
            var lastType = "";
            var temp = new List<string>();
            for(int i = context.typeParameter().Length - 1; i >= 0; i--)
            {
                Parameter p = (Parameter)Visit(context.typeParameter(i));
                if(p.type != null)
                {
                    lastType = p.type;
                }
                else
                {
                    p.type = lastType;
                }
                temp.Add($"{p.type}");
            }
            for(int i = temp.Count - 1; i >= 0; i--)
            {
                if(i == temp.Count - 1)
                {
                    obj += temp[i];
                }
                else
                {
                    obj += $", {temp[i]}";
                }
            }
            return obj;
        }

        public override object VisitTypeParameter([NotNull] XyParser.TypeParameterContext context)
        {
            var p = new Parameter();
            p.id = ((Result)Visit(context.id())).text;
            if(context.type() != null)
            {
                p.type = (string)Visit(context.type());
            }

            return p;
        }

        public override object VisitTypeBasic([NotNull] XyParser.TypeBasicContext context)
        {
            var obj = "";
            switch(context.t.Type)
            {
                case XyParser.TypeI8:
                    obj = "sbyte";
                    break;
                case XyParser.TypeU8:
                    obj = "byte";
                    break;
                case XyParser.TypeI16:
                    obj = "short";
                    break;
                case XyParser.TypeU16:
                    obj = "ushort";
                    break;
                case XyParser.TypeI32:
                    obj = I32;
                    break;
                case XyParser.TypeU32:
                    obj = "uint";
                    break;
                case XyParser.TypeI64:
                    obj = "long";
                    break;
                case XyParser.TypeU64:
                    obj = "ulong";
                    break;
                case XyParser.TypeF32:
                    obj = "float";
                    break;
                case XyParser.TypeF64:
                    obj = F64;
                    break;
                case XyParser.TypeStr:
                    obj = Str;
                    break;
                case XyParser.TypeBool:
                    obj = Bool;
                    break;
                case XyParser.TypeAny:
                    obj = Any;
                    break;
                default:
                    obj = Any;
                    break;
            }
            return obj;
        }
    }
}
