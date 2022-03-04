using Android.App;
using Android.Views;
using DocsBr.Utils;
using DocsBr.Validation;
using EloPedidos.Controllers;
using EloPedidos.Models;
using Plugin.DeviceInfo;
using System;
using System.Globalization;
using static Android.Views.View;

namespace EloPedidos.Utils
{
    public static class Validations
    {
        /// <summary>
        ///  Valida datas com o formato 'dd/MM/yyyy' (dia/mês/ano).
        /// </summary>
        /// <returns><c>true</c>, if validator was date, <c>false</c> otherwise.</returns>
        /// <param name="date">Date.</param>
        public static bool DateValidator(string date)
        {
            try 
            {
                date = date + " 00:00:00";
                if (DateTime.TryParse(date, CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dateModel))
                    return true;
                else
                    return false;
            }
            catch
            {
                Console.WriteLine("Data inválida!");
                return false;
            }
        }

        /// <summary>
        ///  Valida a data e confere se é maior ou igual a do sistema
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool DateValidatorUtilCurrentDate(string date)
        {
            try
            {
                return (DateTime.TryParse(date, CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime outDate) 
                    && (outDate >= DateTime.Now));
            }
            catch
            {
                Console.WriteLine("Data inválida!");
                return false;
            }
        }

        /// <summary>
        ///  Verifica e valida números do cpf
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static bool CPFValidator(string cpf)
        {
            try
            {
                string aux = RemoveMasks.RemoveMasksToString(cpf);

                int v1 = 0; // valor auxiliar 1
                int v2 = 0; // valor auxiliar 2

                if (aux.Length == 11)
                {
                    if (aux.Equals("11111111111") || aux.Equals("22222222222") || aux.Equals("33333333333") ||
                        aux.Equals("44444444444") || aux.Equals("55555555555") || aux.Equals("66666666666") ||
                        aux.Equals("77777777777") || aux.Equals("88888888888") || aux.Equals("99999999999") ||
                        aux.Equals("00000000000"))
                        return false;

                    /*
                     * para o primeiro dígito até o dígito antes de '-' no cpf
                     * é necessário multiplicar cpf[0] * 10, cpf[1] * 9 e assim por diante... 
                     */
                    int i = 0; // iterador para o cpf
                    int j = 10; // multiplicador
                    char[] aux1 = aux.ToCharArray();
                    while (i < 9)
                    {
                        v1 += (int)char.GetNumericValue(aux1[i]) * j;
                        j--;
                        i++;
                    }
                    /*
                     * Nesta parte devemos cálcular o resultado da operação acima * 10 
                     * e receber o resto da divisão por 11, de forma que se for 10 então igualamos a 0,
                     * desta maneira o resto deve ser igual ao primeiro dígito após '-' no cpf
                     */

                    v2 = 11 - (v1 % 11);

                    // Se o resto é 10, igualamos a 0 como na regra
                    if (v2 == 10 || v2 == 11)
                        v2 = 0;

                    /*
                     * se o resto da divisão for igual ao primeiro dígito 
                     * após '-' esta válido o cpf na primeira etapa
                     */
                    if (v2 == (int)char.GetNumericValue(aux1[9]))
                    {
                        /*
                         * A segunda parte da validação é semelhante a primeira,
                         * porém valida o último dígito após '-', e a multiplicação começa com 11, 10, 9 ...
                         * até o 2 dígito assim como a primeira etapa
                         */
                        i = 0; // iterador 2ª parte
                        j = 11; // multiplicador 2ª parte

                        v1 = 0;
                        v2 = 0;
                        aux1 = aux.ToCharArray();

                        while (i < 10)
                        {
                            v1 += (int)char.GetNumericValue(aux1[i]) * j;
                            j--;
                            i++;
                        }

                        v2 = 11 - (v1 % 11);

                        if (v2 == 10 || v2 == 11)
                            v2 = 0;

                        if (v2 == (int)char.GetNumericValue(aux1[10]))
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///  Valida e verifica os números do cnpj
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public static bool CNPJValidator(string cnpj)
        {
            try
            {
                string aux = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

                if (aux.Length == 14)
                {
                    if (aux.Equals("11111111111111") || aux.Equals("22222222222222") || aux.Equals("33333333333333") ||
                        aux.Equals("44444444444444") || aux.Equals("55555555555555") || aux.Equals("66666666666666") ||
                        aux.Equals("77777777777777") || aux.Equals("88888888888888") || aux.Equals("99999999999999") ||
                        aux.Equals("00000000000000"))
                        return false;

                    char[] aux1 = aux.ToCharArray();

                    int count = ((int)char.GetNumericValue(aux[0]) * 5) + ((int)char.GetNumericValue(aux[1]) * 4)
                        + ((int)char.GetNumericValue(aux[2]) * 3) + ((int)char.GetNumericValue(aux[3]) * 2)
                        + ((int)char.GetNumericValue(aux[4]) * 9) + ((int)char.GetNumericValue(aux[5]) * 8)
                        + ((int)char.GetNumericValue(aux[6]) * 7) + ((int)char.GetNumericValue(aux[7]) * 6)
                        + ((int)char.GetNumericValue(aux[8]) * 5) + ((int)char.GetNumericValue(aux[9]) * 4)
                        + ((int)char.GetNumericValue(aux[10]) * 3) + ((int)char.GetNumericValue(aux[11]) * 2);

                    int v1 = (count % 11);

                    if (v1 < 2)
                        v1 = 0;
                    else
                        v1 = (11 - v1);

                    if (v1 == (int)char.GetNumericValue(aux1[12]))
                    {
                        count = 0;
                        count = ((int)char.GetNumericValue(aux[0]) * 6) + ((int)char.GetNumericValue(aux[1]) * 5)
                        + ((int)char.GetNumericValue(aux[2]) * 4) + ((int)char.GetNumericValue(aux[3]) * 3)
                        + ((int)char.GetNumericValue(aux[4]) * 2) + ((int)char.GetNumericValue(aux[5]) * 9)
                        + ((int)char.GetNumericValue(aux[6]) * 8) + ((int)char.GetNumericValue(aux[7]) * 7)
                        + ((int)char.GetNumericValue(aux[8]) * 6) + ((int)char.GetNumericValue(aux[9]) * 5)
                        + ((int)char.GetNumericValue(aux[10]) * 4) + ((int)char.GetNumericValue(aux[11]) * 3)
                        + ((int)char.GetNumericValue(aux[12]) * 2);

                        v1 = 0;
                        v1 = (count % 11);

                        if (v1 < 2)
                            v1 = 0;
                        else
                            v1 = (11 - v1);

                        if (v1 == (int)char.GetNumericValue(aux1[13]))
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///  Impede a digitação de letras
        /// </summary>
        /// <param name="edit"></param>
        /// <param name="args"></param>
        public static void DecimalEntry(Android.Widget.EditText edit, KeyEventArgs args)
        {
            if ((args.KeyCode == Keycode.Comma && edit.Text.Contains(",")) || 
                (args.KeyCode == Keycode.Comma && string.IsNullOrEmpty(edit.Text)))
                args.Handled = true;
            else
                args.Handled = false;
        }

        public static bool ValidadorCNPJ(string cnpj)
        {
            return new CNPJValidator(cnpj).IsValid();
        }
        public static bool ValidadorCPF(string cpf)
        {
            return new CPFValidator(cpf).IsValid();
        }
        public static bool ValidadorIE(string ie, string uf)
        {
            return new IEValidator(ie, UF.ToEnum(uf)).IsValid();
        }

        //public static void VerificarPermissao(this Activity context)
        //{
        //    if (new ConfigController().TestServerConnection("elosoftware.dyndns.org", 8560))
        //    {
        //        ConfigController cController = new ConfigController();
        //        Models.Config config = cController.GetConfig();

        //        if (config.DTHULTVER != null)
        //        {
        //           double ultimaVerificacao = DateTime.Now.Subtract(config.DTHULTVER).TotalDays;

        //            if (ultimaVerificacao >= 1 || !config.isAuthorized)
        //            {
        //                Aparelho a = null;

        //                if (new AparelhoController().FindAll().Count == 0)
        //                {
        //                    var device = CrossDeviceInfo.Current;
        //                    a = new Aparelho();
        //                    a.ID_APARELHO = device.Id;
        //                    a.DSCAPAR = device.DeviceName;
        //                    a.NOMOPER = new OperadorController().GetOperador().USROPER;
        //                    a.IDTPESS = new EmpresaController().GetEmpresa().NROCNPJ;
        //                    a.NROVERS = "0";
        //                    a.TIPSAPAR = "1";
        //                    a.INDINAT = "0";
        //                    a.DTHULTAT = DateTime.Now;
        //                    a.USRULTAT = new OperadorController().GetOperador().USROPER;

        //                    if (new AparelhoController().Insert(a))
        //                        new AparelhoController().syncDevice(a);
        //                }
        //                else
        //                {
        //                    a = new AparelhoController().GetAparelho();
        //                    if (!a.INDSYNC)
        //                        new AparelhoController().syncDevice(a);
        //                }

        //                if (a.INDSYNC)
        //                {
        //                    bool permission = new ConfigController().applicationPermission(out string error);
        //                    if (permission)
        //                    {
        //                        config.isAuthorized = true;
        //                        config.DTHULTVER = DateTime.Now;
        //                        cController.Save(config);
        //                    }
        //                    else
        //                    {
        //                        context.RunOnUiThread(() =>
        //                        {
        //                            Android.Support.V7.App.AlertDialog.Builder bd = new Android.Support.V7.App.AlertDialog.Builder(context);
        //                            bd.SetTitle("AVISO DO SISTEMA !");
        //                            bd.SetMessage("SISTEMA ATUALMENTE BLOQUEADO \n" + error + "\n" + "CONTATE O ADMINISTRADOR DO SISTEMA");
        //                            bd.SetPositiveButton("OK", (s, a) =>
        //                            {
        //                                config.DSCRERRO = error;
        //                                config.isAuthorized = false;
        //                                cController.Save(config);
        //                                context.Finish();
        //                            });
        //                            bd.SetCancelable(false);
        //                            Android.Support.V7.App.AlertDialog alert = bd.Create();
        //                            alert.Show();

        //                        });
        //                    }
        //                }
        //                else if (!config.isAuthorized)
        //                {
        //                    context.RunOnUiThread(() =>
        //                    {
        //                        Android.Support.V7.App.AlertDialog.Builder bd = new Android.Support.V7.App.AlertDialog.Builder(context);
        //                        bd.SetTitle("AVISO DO SISTEMA !");
        //                        bd.SetMessage("SISTEMA ATUALMENTE BLOQUEADO \n" + "APARELHO NÃO CADASTRADO" + "\n" + "CONTATE O ADMINISTRADOR DO SISTEMA");
        //                        bd.SetPositiveButton("OK", (s, a) =>
        //                        {
        //                            context.Finish();
        //                        });
        //                        bd.SetCancelable(false);
        //                        Android.Support.V7.App.AlertDialog alert = bd.Create();
        //                        alert.Show();

        //                    });
        //                }

        //            }
        //        }
        //    }
        //}

        public static void VerificarPermissao(this Activity context)
        {
            if (new ConfigController().TestServerConnection("elosoftware.dyndns.org", 8560))
            {
                ConfigController cController = new ConfigController();
                Models.Config config = cController.GetConfig();

                bool autorizacao = true;
                DateTime? data = null;

                string strALT = Ext.RestorePreference(context, "ISAUTHORIZED");
                if (!string.IsNullOrEmpty(strALT))
                    autorizacao = strALT.stringToBoolean();

                string strDATA = Ext.RestorePreference(context, "DTHULVER");
                if (!string.IsNullOrEmpty(strDATA))
                    data = DateTime.Parse(strDATA);

                if (!string.IsNullOrEmpty(strALT) && !string.IsNullOrEmpty(strDATA))
                {
                    double difhora = DateTime.Now.Subtract(data.Value).TotalDays;

                    if (difhora >= 1)
                    {
                        Aparelho a = null;

                        if (new AparelhoController().FindAll().Count == 0)
                        {
                            var device = CrossDeviceInfo.Current;
                            a = new Aparelho();
                            a.ID_APARELHO = device.Id;
                            a.DSCAPAR = device.DeviceName;
                            a.NOMOPER = new OperadorController().GetOperador().USROPER;
                            a.IDTPESS = new EmpresaController().GetEmpresa().NROCNPJ;
                            a.NROVERS = "0";
                            a.TIPSAPAR = "3";
                            a.INDINAT = "0";
                            a.DTHULTAT = DateTime.Now;
                            a.USRULTAT = new OperadorController().GetOperador().USROPER;

                            if (new AparelhoController().Insert(a))
                                if (!new AparelhoController().syncDevice(a, out string message))
                                    if (message == "UNABLE TO READ")
                                        return;
                        }
                        else
                        {
                            a = new AparelhoController().GetAparelho();
                            if (!a.INDSYNC)
                                if (!new AparelhoController().syncDevice(a, out string message))
                                    if (message == "UNABLE TO READ")
                                        return;
                        }

                        if (a.INDSYNC)
                        {
                            bool permission = new ConfigController().applicationPermission(out string error);
                            if (permission)
                            {
                                Ext.SavePreference(context, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "DTHULVER");
                                Ext.SavePreference(context, "1", "ISAUTHORIZED");

                            }
                            else
                            {
                                if (error == "UNABLE TO READ")
                                    return;
                                else
                                    context.RunOnUiThread(() =>
                                    {
                                        Android.Support.V7.App.AlertDialog.Builder bd = new Android.Support.V7.App.AlertDialog.Builder(context);
                                        bd.SetTitle("AVISO DO SISTEMA !");
                                        bd.SetMessage("SISTEMA ATUALMENTE BLOQUEADO \n" + error + "\n" + "CONTATE O ADMINISTRADOR DO SISTEMA");
                                        bd.SetPositiveButton("OK", (s, a) =>
                                        {
                                           // Ext.SavePreference(context, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "DTHULVER");
                                           // Ext.SavePreference(context, "0", "ISAUTHORIZED");

                                            //config.DSCRERRO = error;
                                            //config.isAuthorized = false;
                                            //cController.Save(config);
                                            context.Finish();
                                        });
                                        bd.SetCancelable(false);
                                        Android.Support.V7.App.AlertDialog alert = bd.Create();
                                        alert.Show();

                                    });
                            }
                        }
                        else if (!autorizacao)
                        {
                            context.RunOnUiThread(() =>
                            {
                                Android.Support.V7.App.AlertDialog.Builder bd = new Android.Support.V7.App.AlertDialog.Builder(context);
                                bd.SetTitle("AVISO DO SISTEMA !");
                                bd.SetMessage("SISTEMA ATUALMENTE BLOQUEADO \n" + "APARELHO NÃO CADASTRADO" + "\n" + "CONTATE O ADMINISTRADOR DO SISTEMA");
                                bd.SetPositiveButton("OK", (s, a) =>
                                {
                                    context.Finish();
                                });
                                bd.SetCancelable(false);
                                Android.Support.V7.App.AlertDialog alert = bd.Create();
                                alert.Show();

                            });
                        }
                    }
                }
                else
                {
                    Ext.SavePreference(context, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "DTHULVER");
                    Ext.SavePreference(context, "1", "ISAUTHORIZED");
                }
            }
        }
    }
}
