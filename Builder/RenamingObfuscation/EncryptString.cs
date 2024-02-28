using Server.RenamingObfuscation.Classes;
using Server.RenamingObfuscation.Interfaces;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MetroFramework.Controls;
using System.IO;

namespace Builder.RenamingObfuscation
{

    public static class EncryptString
    {

        private static MethodDef InjectMethod(ModuleDef module, string methodName)
        {
            ModuleDefMD typeModule = ModuleDefMD.Load(typeof(DecryptionHelper).Module);
            TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(DecryptionHelper).MetadataToken));
            IEnumerable<IDnlibDef> members = InjectHelper.Inject(typeDef, module.GlobalType, module);
            MethodDef injectedMethodDef = (MethodDef)members.Single(method => method.Name == methodName);

            foreach (MethodDef md in module.GlobalType.Methods)
            {
                if (md.Name == ".ctor")
                {
                    module.GlobalType.Remove(md);
                    break;
                }
            }

            return injectedMethodDef;
        }

        public static void DoEncrypt(ModuleDef inPath)
        {
            EncryptStrings(inPath);
        }


       
        private static ModuleDef EncryptStrings(ModuleDef inModule)
        {
            ModuleDef module = inModule;
            MethodDef decryptMethod = InjectMethod(module, "Update");

            foreach (TypeDef type in module.Types)
            {
                if (type.IsGlobalModuleType || type.Name == "Resources" || type.Name == "Settings")
                    continue;

                foreach (MethodDef method in type.Methods)
                {
                    if (!method.HasBody)
                        continue;
                    if (method == decryptMethod)
                        continue;

                    method.Body.KeepOldMaxStack = true;

                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                    {
                        if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                        {
                            string oldString = method.Body.Instructions[i].Operand.ToString();	
                            method.Body.Instructions[i].Operand = XOR_Encrypt(oldString, Form1.keystring);
                            method.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Call, decryptMethod));
                        }
                    }

                    method.Body.SimplifyBranches();
                    method.Body.OptimizeBranches();
                }
            }

            foreach (TypeDef type in module.Types)
            {

                if (type.Name == "<Module>")
                    foreach (MethodDef method in type.Methods)
                    {
                        if (method.Body == null) continue;
                        for (int i = 0; i < method.Body.Instructions.Count(); i++)
                        {
                            if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr)
                            {
                                if (method.Body.Instructions[i].Operand.ToString() == "[KEY]")
                                    method.Body.Instructions[i].Operand = Form1.keystring;
                            }
                        }
                    }
            }

            return module;
        }

       

        public static string XOR_Encrypt(string whatToencrypt, string key)
        {
            List<byte> resStr = new List<byte>();
            int i = 0;
            foreach (var c in whatToencrypt)
            {
                resStr.Add((byte)(c ^ key[i++]));
                i = i % key.Length;
            }
            whatToencrypt = System.Text.Encoding.Default.GetString(resStr.ToArray());
            return whatToencrypt;
        }


       
    }

    
}
