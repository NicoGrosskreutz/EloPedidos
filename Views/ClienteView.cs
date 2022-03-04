using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Models;
using EloPedidos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using EloPedidos.Persistence;
using Android.Support.Design.Widget;
using System.Threading.Tasks;

namespace EloPedidos.Views
{
    [Activity(Label = "Cadastro Cliente")]
    public class ClienteView : Activity
    {
        private RelativeLayout RLayout;
        private EditText txCODPESS;
        private TextInputEditText txNOMPESS;
        private TextInputEditText txNOMFANTA;
        private TextInputEditText txIDTPESS;
        private TextInputEditText txNROINEST;
        private TextInputEditText txDSCENDER;
        private TextInputEditText txNROENDER;
        private TextInputEditText txCPLENDER;
        private TextInputEditText txNOMBAIRR;
        private TextInputEditText txNROCEP;
        private TextInputEditText txCODMUNIC;
        private TextInputEditText txNOMMUNIC;
        private TextInputEditText txNROCELUL;
        private TextInputEditText txNROFONEC;
        private Button btnSalvar;
        private Button btnConsultaCNPJ;
        private Button btnLimpar;
        private Button btnExcluir;
        private RadioButton rbCPF, rbOUTRO, rbCNPJ;
        private RadioGroup radioGroup;
        /// <summary>
        ///  Id da pessoa para referência na view
        /// </summary>
        //public long? CG_PESSOA_ID { get; set; } = null;
        public long? ID { get; set; } = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_cadastrarCliente);

            RLayout = FindViewById<RelativeLayout>(Resource.Id.relativeLayout);
            txCODPESS = FindViewById<EditText>(Resource.Id.txCODPESS);
            txNOMPESS = FindViewById<TextInputEditText>(Resource.Id.txNOMPESS);
            txNOMFANTA = FindViewById<TextInputEditText>(Resource.Id.txNOMFANTA);
            txIDTPESS = FindViewById<TextInputEditText>(Resource.Id.txIDTPESS);
            txNROINEST = FindViewById<TextInputEditText>(Resource.Id.txNROINEST);
            txDSCENDER = FindViewById<TextInputEditText>(Resource.Id.txDSCENDER);
            txNROENDER = FindViewById<TextInputEditText>(Resource.Id.txNROENDER);
            txCPLENDER = FindViewById<TextInputEditText>(Resource.Id.txCPLENDER);
            txNOMBAIRR = FindViewById<TextInputEditText>(Resource.Id.txNOMBAIRR);
            txNROCEP = FindViewById<TextInputEditText>(Resource.Id.txNROCEP);
            txCODMUNIC = FindViewById<TextInputEditText>(Resource.Id.txCODMUNIC);
            txNOMMUNIC = FindViewById<TextInputEditText>(Resource.Id.txNOMMUNIC);
            txNROCELUL = FindViewById<TextInputEditText>(Resource.Id.txNROCELUL);
            txNROFONEC = FindViewById<TextInputEditText>(Resource.Id.txNROFONEC);
            btnSalvar = FindViewById<Button>(Resource.Id.btnSalvar);
            btnConsultaCNPJ = FindViewById<Button>(Resource.Id.btnConsultaCNPJ);
            btnLimpar = FindViewById<Button>(Resource.Id.btnLimpar);
            btnExcluir = FindViewById<Button>(Resource.Id.btnExcluir);
            rbCPF = FindViewById<RadioButton>(Resource.Id.rbCPF);
            rbOUTRO = FindViewById<RadioButton>(Resource.Id.rbOUTRO);
            rbCNPJ = FindViewById<RadioButton>(Resource.Id.rbCNPJ);
            radioGroup = FindViewById<RadioGroup>(Resource.Id.radioGroup);

            txNOMFANTA.Enabled = false;
            txNOMMUNIC.Enabled = false;
            btnConsultaCNPJ.Enabled = false;

            if (Intent.HasExtra("CODPESS"))
            {
                string codpess = Intent.GetStringExtra("CODPESS");
                txCODPESS.Text = codpess;
                if (new PessoaController().FindByCODPESS(long.Parse(txCODPESS.Text)) != null)
                    LoadDataFromCODPESS(long.Parse(txCODPESS.Text));
            }

            /* Eventos */
            txCODMUNIC.TextChanged += (sender, eventArgs) =>
            {
                if (!string.IsNullOrEmpty(txCODMUNIC.Text) || !string.IsNullOrWhiteSpace(txCODMUNIC.Text))
                {
                    string fNOMMUNIC = new MunicipioController().FindNameById(long.Parse(txCODMUNIC.Text));
                    txNOMMUNIC.Text = !string.IsNullOrEmpty(fNOMMUNIC) ? fNOMMUNIC : "Município não encontrado!";
                }

                if (string.IsNullOrEmpty(txCODMUNIC.Text)) txNOMMUNIC.Text = "";
            };
            txCODPESS.TextChanged += (sender, eventArgs) =>
            {
                if (!string.IsNullOrEmpty(txCODPESS.Text))
                    if (new PessoaController().FindByCODPESS(long.Parse(txCODPESS.Text)) != null)
                        LoadDataFromCODPESS(long.Parse(txCODPESS.Text));

            };
            txCODPESS.FocusChange += (sender, eventArgs) =>
            {
                if (!eventArgs.HasFocus && !string.IsNullOrEmpty(txCODPESS.Text))
                {
                    if (new PessoaController().FindByCODPESS(long.Parse(txCODPESS.Text)) == null)
                    {
                        this.Msg("CÓDIGO DE CLIENTE INVÁLIDO!");
                        txCODPESS.Text = string.Empty;
                    }
                    txCODPESS.SetSelectAllOnFocus(true);
                }
            };
            txCODPESS.LongClick += (sender, eventArgs) =>
            {
                if (new PessoaDAO().FindAll().Count == 0)
                    this.Msg("NENHUM CLIENTE CADASTRADO!");
                else
                {
                    Intent i = new Intent(Application.Context, typeof(BuscaClienteView));
                    StartActivityForResult(i, 1);
                }
            };
            txCODMUNIC.LongClick += (sender, eventArgs) =>
            {
                if (new MunicipioController().FindAll().Count == 0)
                    this.Msg("NENHUM MUNICIPIO REGISTRADO!");
                else
                {
                    Intent i = new Intent(Application.Context, typeof(BuscaMunicipioView));
                    StartActivityForResult(i, 1);
                }
            };
            btnConsultaCNPJ.Click += (sender, args) =>
            {
                if (txIDTPESS.Text != "")
                {
                    if (txIDTPESS.Text.Length >= 14)
                    {
                        string cnpj = Utils.RemoveMasks.RemoveMasksToString(txIDTPESS.Text);
                        if (Utils.Validations.ValidadorCNPJ(cnpj))
                            consultarCPNJ(cnpj);
                        else
                            this.Msg("CNPJ INSERIDO INVÁLIDO");
                    }
                    else
                        this.Msg("CNPJ INSERIDO INVÁLIDO");
                }
                else
                    this.Msg("FAVOR INSERIR CNPJ A CONSULTAR !");
            };
            txIDTPESS.FocusChange += (sender, args) =>
            {
                if (!args.HasFocus)
                {
                    if (!string.IsNullOrEmpty(txIDTPESS.Text))
                    {
                        string idtpess = RemoveMasks.RemoveMasksToString(txIDTPESS.Text);
                        formatIDTPESS(txIDTPESS.Text);
                        txIDTPESS.SetSelectAllOnFocus(true);

                        Pessoa pessoa = new PessoaController().FindByIDTPESS(idtpess);
                        if (pessoa != null)
                            txCODPESS.Text = pessoa.CODPESS.ToString();

                        if (rbCNPJ.Checked)
                        {
                            if (!Utils.Validations.ValidadorCNPJ(txIDTPESS.Text))
                            {
                                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                                builder.SetTitle("AVISO");
                                builder.SetMessage("CNPJ INCORRETO, VERIFIQUE");
                                builder.SetCancelable(false);
                                builder.SetPositiveButton("OK", (s, a) =>
                                {
                                    LimparTela();
                                    txIDTPESS.SelectAll();
                                    txIDTPESS.SetSelectAllOnFocus(true);
                                    txIDTPESS.RequestFocus();
                                    return;
                                });
                                AlertDialog alert = builder.Create();
                                alert.Show();
                            }
                        }
                        if (rbCPF.Checked)
                        {
                            if (!Utils.Validations.ValidadorCPF(txIDTPESS.Text))
                            {
                                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                                builder.SetTitle("AVISO");
                                builder.SetMessage("CPF INCORRETO, VERIFIQUE");
                                builder.SetCancelable(false);
                                builder.SetPositiveButton("OK", (s, a) =>
                                {
                                    LimparTela();
                                    txIDTPESS.SelectAll();
                                    txIDTPESS.SetSelectAllOnFocus(true);
                                    txIDTPESS.RequestFocus();
                                    return;
                                });
                                AlertDialog alert = builder.Create();
                                alert.Show();
                            }
                        }
                    }


                }
                else
                {
                    if (!string.IsNullOrEmpty(txIDTPESS.Text))
                    {
                        txIDTPESS.Text = RemoveMasks.RemoveMasksToString(txIDTPESS.Text);
                    }
                }
            };

            btnExcluir.Click += (s, a) => ExcluirCadastro();
            txNOMPESS.FocusChange += (s, a) =>
            {
                if (!txNOMPESS.HasFocus)
                    if (radioGroup.CheckedRadioButtonId == Resource.Id.rbCPF || radioGroup.CheckedRadioButtonId == Resource.Id.rbOUTRO)
                        txNOMFANTA.Text = txNOMPESS.Text;
            };

            radioGroup.CheckedChange += (sender, args) =>
            {
                if (radioGroup.CheckedRadioButtonId == Resource.Id.rbCPF)
                {
                    txNOMFANTA.Enabled = false;
                    btnConsultaCNPJ.Enabled = false;
                }
                else if (radioGroup.CheckedRadioButtonId == Resource.Id.rbOUTRO)
                {
                    txNOMFANTA.Enabled = false;
                    btnConsultaCNPJ.Enabled = false;
                }
                else if (radioGroup.CheckedRadioButtonId == Resource.Id.rbCNPJ)
                {
                    txNOMFANTA.Enabled = true;
                    btnConsultaCNPJ.Enabled = true;
                }
            };


            btnSalvar.Click += (sender, args) =>
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("CADASTRO DE CLIENTE");
                builder.SetMessage("GOSTÁRIA DE SALVAR ?");
                builder.SetPositiveButton("SIM", (s, a) => SalvarCliente());
                builder.SetNegativeButton("NÃO", (s, a) => { return; });
                AlertDialog dialog = builder.Create();
                dialog.Show();
            };
            btnLimpar.Click += (sender, args) =>
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("AVISO DO SISTEMA");
                builder.SetMessage("DESEJA MESMO LIMPAR DADOS?");
                builder.SetPositiveButton("OK", (s, a) =>
                {
                    LimparTela();

                });
                builder.SetNegativeButton("NÂO", (sender, args) => { return; });
                AlertDialog alert = builder.Create();
                alert.Show();
                LimparFocus();
                txCODMUNIC.RequestFocus();
            };
            /* ======= */
            txNOMFANTA.FocusChange += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txNOMFANTA.Text))
                    txNOMFANTA.SetSelectAllOnFocus(true);
            };
            txNOMPESS.FocusChange += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txNOMPESS.Text))
                    txNOMPESS.SetSelectAllOnFocus(true);
            };
            txNROINEST.FocusChange += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txNROINEST.Text))
                    txNROINEST.SetSelectAllOnFocus(true);
            };
            txDSCENDER.FocusChange += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txDSCENDER.Text))
                    txDSCENDER.SetSelectAllOnFocus(true);
            };
            txNROENDER.FocusChange += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txNROENDER.Text))
                    txNROENDER.SetSelectAllOnFocus(true);
            };
            txCPLENDER.FocusChange += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txCPLENDER.Text))
                    txCPLENDER.SetSelectAllOnFocus(true);
            };
            txNOMBAIRR.FocusChange += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txNOMBAIRR.Text))
                    txNOMBAIRR.SetSelectAllOnFocus(true);
            };
            txNROCEP.FocusChange += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txNROCEP.Text))
                    txNROCEP.SetSelectAllOnFocus(true);
                if (txNROCEP.Text != "")
                    if (!args.HasFocus)
                        if (txNROCEP.Text.Length == 8)
                            txNROCEP.Text = Convert.ToUInt64(txNROCEP.Text).ToString(@"00\.000\-000");
            };
            txCODMUNIC.FocusChange += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txCODMUNIC.Text))
                    txCODMUNIC.SetSelectAllOnFocus(true);
            };
            txNROCELUL.FocusChange += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txNROCELUL.Text))
                    txNROCELUL.SetSelectAllOnFocus(true);
                if (txNROCELUL.Text != "")
                    if (!args.HasFocus)
                    {
                        string aux = RemoveMasks.RemoveMasksToString(txNROCELUL.Text);
                        if (aux.Length == 8)
                            txNROCELUL.Text = Convert.ToUInt64(aux).ToString(@"0000\-0000");
                        else if (aux.Length == 9)
                            txNROCELUL.Text = Convert.ToUInt64(aux).ToString(@"00000\-0000");
                        else if (aux.Length == 10)
                            txNROCELUL.Text = Convert.ToUInt64(aux).ToString(@"(00)0000\-0000");
                        else if (aux.Length == 11)
                            txNROCELUL.Text = Convert.ToUInt64(aux).ToString(@"(00)00000\-0000");

                    }
            };
            txNROFONEC.FocusChange += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txNROFONEC.Text))
                    txNROFONEC.SetSelectAllOnFocus(true);
                if (txNROFONEC.Text != "")
                    if (!args.HasFocus)
                    {
                        string aux = RemoveMasks.RemoveMasksToString(txNROFONEC.Text);
                        if (aux.Length == 8)
                            txNROFONEC.Text = Convert.ToUInt64(aux).ToString(@"0000\-0000");
                        else if (aux.Length == 9)
                            txNROFONEC.Text = Convert.ToUInt64(aux).ToString(@"00000\-0000");
                        else if (aux.Length == 10)
                            txNROFONEC.Text = Convert.ToUInt64(aux).ToString(@"(00)0000\-0000");
                        else if (aux.Length == 11)
                            txNROFONEC.Text = Convert.ToUInt64(aux).ToString(@"(00)00000\-0000");

                        HideKeyboard(txNROFONEC);
                    }
            };
            txNROINEST.FocusChange += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txNROINEST.Text))
                    txNROINEST.SetSelectAllOnFocus(true);

                if (!string.IsNullOrEmpty(txNROINEST.Text))
                    if (!txNROINEST.HasFocus)
                        if (!string.IsNullOrEmpty(txCODMUNIC.Text))
                        {
                            Municipio municipio = new MunicipioController().FindById(txCODMUNIC.Text.ToLong());
                            if (municipio != null)
                            {
                                if (!Validations.ValidadorIE(txNROINEST.Text, municipio.CODUF))
                                {
                                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                                    builder.SetTitle("AVISO");
                                    builder.SetMessage("INSCRIÇÃO ESTADUAL INCORRETO, VERIFIQUE");
                                    builder.SetCancelable(false);
                                    builder.SetPositiveButton("OK", (s, a) =>
                                    {
                                        txNROINEST.SelectAll();
                                        txNROINEST.SetSelectAllOnFocus(true);
                                        txNROINEST.RequestFocus();
                                        return;
                                    });
                                    AlertDialog alert = builder.Create();
                                    alert.Show();
                                }
                            }
                        }
            };
        }

        private void SalvarCliente()
        {
            try
            {
                Municipio municipio = new MunicipioController().FindById(txCODMUNIC.Text.ToLong());
                TextInputEditText[] dados = ValidarDados();
                if (dados.Length > 0)
                {
                    LimparFocus();
                    dados[0].RequestFocus();
                    dados[0].SetError("CAMPO OBRIGATÓRIO !", null);
                }
                else if (radioGroup.CheckedRadioButtonId == Resource.Id.rbCPF && !Validations.ValidadorCPF(txIDTPESS.Text))
                {
                    LimparFocus();
                    txIDTPESS.RequestFocus();
                    txIDTPESS.SetError("CPF INCORRETO !", null);
                }
                else if (radioGroup.CheckedRadioButtonId == Resource.Id.rbCNPJ && !Validations.ValidadorCNPJ(txIDTPESS.Text))
                {
                    LimparFocus();
                    txIDTPESS.RequestFocus();
                    txIDTPESS.SetError("CNPJ INCORRETO !", null);
                }
                else if (radioGroup.CheckedRadioButtonId == Resource.Id.rbCNPJ && !string.IsNullOrEmpty(txNROINEST.Text) && !Validations.ValidadorIE(txNROINEST.Text, municipio.CODUF))
                {
                    LimparFocus();
                    txNROINEST.RequestFocus();
                    txNROINEST.SetError("INSCRIÇÃO ESTADUAL INCORRETO !", null);
                }
                else if (!string.IsNullOrEmpty(txCODPESS.Text) && txCODPESS.Text != "0" && new PessoaController().FindByCODPESS(string.IsNullOrEmpty(txCODPESS.Text) ? 0 : TryParseLong(txCODPESS.Text)) == null)
                {
                    LimparFocus();
                    txCODPESS.RequestFocus();
                    txCODPESS.SetError("CLIENTE NÃO ENCONTRADO! !", null);
                }
                else if (!string.IsNullOrEmpty(txCODMUNIC.Text) && new MunicipioController().FindById(TryParseLong(txCODMUNIC.Text)) == null)
                {
                    LimparFocus();
                    txCODMUNIC.RequestFocus();
                    txCODMUNIC.SetError("CÓDIGO DE MUNICIPIO INVÁLIDO !", null);
                }
                else if (radioGroup.CheckedRadioButtonId == Resource.Id.rbOUTRO)
                {
                    txIDTPESS.RequestFocus();
                    txIDTPESS.SetError("FAVOR INSERIR UM CPF OU CNPJ !", null);
                }
                else if (!string.IsNullOrEmpty(txNROCEP.Text) && ((txNROCEP.Text.Contains(".") && txNROCEP.Text.Length != 10) || (!txNROCEP.Text.Contains(".") && txNROCEP.Text.Length != 8)))
                {
                    txNROCEP.RequestFocus();
                    txNROCEP.SetError("CEP INFORMADO ESTÁ INCORRETO !", null);
                }
                else
                {
                    short INDDOC;

                    if (radioGroup.CheckedRadioButtonId == Resource.Id.rbCPF)
                    {
                        if (txIDTPESS.Text.Length >= 11)
                            INDDOC = 1;
                        else
                            INDDOC = 2;
                    }
                    else if (radioGroup.CheckedRadioButtonId == Resource.Id.rbCNPJ)
                    {
                        if (txIDTPESS.Text.Length >= 14)
                            INDDOC = 0;
                        else
                            INDDOC = 2;
                    }
                    else
                    {
                        INDDOC = 2;
                    }

                    Pessoa p = null;
                    p = new Pessoa()
                    {
                        CODEMPRE = new EmpresaController().GetEmpresa().CODEMPRE,
                        NOMPESS = txNOMPESS.Text,
                        NOMFANTA = txNOMFANTA.Text,
                        DSCENDER = txDSCENDER.Text,
                        NROENDER = txNROENDER.Text,
                        CPLENDER = txCPLENDER.Text,
                        NOMBAIRR = txNOMBAIRR.Text,
                        NROCELUL = txNROCELUL.Text,
                        NROFONEC = txNROFONEC.Text,
                        IDTDCPES = INDDOC,
                        DTHULTAT = DateTime.Now,
                        USRULTAT = new OperadorController().GetOperador().USROPER,
                        INDSINC = false
                    };

                    Pessoa pess = null;
                    if ((pess = new PessoaController().FindByIDTPESS(RemoveMasks.RemoveMasksToString(txIDTPESS.Text))) != null || !string.IsNullOrEmpty(txCODPESS.Text))
                    {
                        if (pess == null)
                            pess = new PessoaController().FindByCODPESS(long.Parse(txCODPESS.Text));

                        if (pess != null)
                        {
                            p.ID = pess.ID;
                            p.CG_PESSOA_ID = pess.CG_PESSOA_ID;
                            p.CODPESS = pess.CODPESS;
                        }
                    }

                    if (!string.IsNullOrEmpty(txIDTPESS.Text))
                    {
                        p.IDTPESS = RemoveMasks.RemoveMasksToString(txIDTPESS.Text);

                        switch (p.IDTPESS.Length)
                        {
                            case 11:
                                p.IDTDCPES = (short)Pessoa.TipoDocumento.CPF;
                                break;
                            case 14:
                                p.IDTDCPES = (short)Pessoa.TipoDocumento.CNPJ;
                                break;
                        }
                    }

                    if (!string.IsNullOrEmpty(txNROINEST.Text))
                        p.NROINEST = txNROINEST.Text;

                    if (!string.IsNullOrEmpty(txCODMUNIC.Text))
                        p.CODMUNIC = TryParseLong(txCODMUNIC.Text);

                    if (!string.IsNullOrEmpty(txNOMMUNIC.Text))
                        p.NOMMUNIC = municipio.NOMMUNIC;

                    if (!string.IsNullOrEmpty(txNROCEP.Text))
                        p.NROCEP = TryParseLong(txNROCEP.Text);

                    if (new PessoaController().Save(p))
                    {
                        this.SnackMsg("CLIENTE SALVO COM SUCESSO!");
                        LimparTela();
                        LoadDataFromId(p.ID.Value);
                    }
                    else
                        this.SnackMsg("ERRO AO SALVAR CLIENTE! VERIFIQUE.");
                }
            }
            catch (Exception ex)
            {
                GetError(ex.ToString());
            }
        }

        private TextInputEditText[] ValidarDados()
        {
            List<TextInputEditText> lista = new List<TextInputEditText>();

            if (string.IsNullOrEmpty(txIDTPESS.Text))
                lista.Add(txIDTPESS);
            if (string.IsNullOrEmpty(txNOMPESS.Text))
                lista.Add(txNOMPESS);
            if (string.IsNullOrEmpty(txDSCENDER.Text))
                lista.Add(txDSCENDER);
            if (string.IsNullOrEmpty(txNROENDER.Text))
                lista.Add(txNROENDER);
            if (string.IsNullOrEmpty(txNOMBAIRR.Text))
                lista.Add(txNOMBAIRR);
            if (string.IsNullOrEmpty(txCODMUNIC.Text))
                lista.Add(txCODMUNIC);

            return lista.ToArray();

        }

        private void ExcluirCadastro()
        {
            Pessoa pessoa = null;

            if(string.IsNullOrEmpty(txIDTPESS.Text) && string.IsNullOrEmpty(txCODPESS.Text))
            {
                this.Msg("FAVOR SELECIONE O CLIENTE");
                return;
            }

            if ((pessoa = new PessoaController().FindByIDTPESS(RemoveMasks.RemoveMasksToString(txIDTPESS.Text))) != null || !string.IsNullOrEmpty(txCODPESS.Text))
            {
                if (pessoa == null)
                    pessoa = new PessoaController().FindByCODPESS(long.Parse(txCODPESS.Text));
            }

            if (pessoa == null)
                this.Msg("PESSOA NÃO ENCONTRADA");
            else
            {
                if (!pessoa.CG_PESSOA_ID.HasValue || new PedidoController().FindByCG_PESSOA_ID(pessoa.CG_PESSOA_ID.Value).Count == 0)
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("AVISO!");
                    builder.SetMessage($"({pessoa.NOMFANTA})\nGOSTARIA DE EXCLUIR ESSE CLIENTE ?");
                    builder.SetPositiveButton("SIM", (s, a) =>
                    {
                        new PessoaController().Delete(pessoa.ID.Value);
                        LimparTela();
                    });
                    builder.SetNegativeButton("NÃO", (s, a) => { return; });
                    AlertDialog dialog = builder.Create();
                    dialog.Show();
                }
                else
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("AVISO!");
                    builder.SetMessage($"NÃO É POSSÍVEL EXCLUIR O CADASTRO, HÁ PEDIDOS NESSE CLIENTE NA MEMÓRIA");
                    builder.SetPositiveButton("OK", (s, a) => { return; });
                    AlertDialog dialog = builder.Create();
                    dialog.Show();
                }
            }
        }

        private void formatIDTPESS(string doc)
        {
            string auxIDTPESS = RemoveMasks.RemoveMasksToString(doc);

            if (auxIDTPESS.Length == 14)
            {
                txIDTPESS.Text = Convert.ToUInt64(txIDTPESS.Text).ToString(@"00\.000\.000\/0000\-00");
                radioGroup.Check(Resource.Id.rbCNPJ);
            }
            else if (auxIDTPESS.Length == 11)
            {
                txIDTPESS.Text = Convert.ToUInt64(txIDTPESS.Text).ToString(@"000\.000\.000\-00");
                radioGroup.Check(Resource.Id.rbCPF);
            }
            else if (auxIDTPESS.Length > 0 && radioGroup.CheckedRadioButtonId == Resource.Id.rbOUTRO)
                radioGroup.Check(Resource.Id.rbOUTRO);

            if (auxIDTPESS.Length == 15 && radioGroup.CheckedRadioButtonId == Resource.Id.rbCPF)
            {
                if (!Validations.ValidadorCPF(txIDTPESS.Text))
                    this.Msg("CPF INVÁLIDO! VERIFIQUE.");
            }
            else if (auxIDTPESS.Length == 18 && radioGroup.CheckedRadioButtonId == Resource.Id.rbCNPJ)
            {
                if (!Validations.ValidadorCNPJ(txIDTPESS.Text))
                    this.Msg("CNPJ INVÁLIDO! VERIFIQUE.");
            }
        }

        public static void HideKeyboard(TextInputEditText editText)
        {
            InputMethodManager inputMethodManager = Application.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.HideSoftInputFromWindow(editText.WindowToken, HideSoftInputFlags.None);
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 1)
                if (resultCode == Result.Ok)
                {
                    string result = data.GetStringExtra("result");
                    if (!string.IsNullOrEmpty(result))
                    {
                        var id = data.GetStringExtra("result");
                        //Pessoa pessoa = new PessoaController().FindByIDTPESS(RemoveMasks.RemoveMasksToString(doc));
                        Pessoa pessoa = new PessoaController().FindById(id.ToLong());
                        if (pessoa != null)
                        {
                            this.ID = pessoa.ID;
                            if (pessoa.CODPESS.HasValue)
                                txCODPESS.Text = pessoa.CODPESS.ToString();
                            LoadDataFromId(this.ID);
                        }
                    }

                    string municResult = data.GetStringExtra("municResult");
                    if (!string.IsNullOrEmpty(municResult))
                        txCODMUNIC.Text = data.GetStringExtra("municResult");
                }

            // deletar cliente
            if (requestCode == 2)
                if (resultCode == Result.Ok)
                {
                    string result = data.GetStringExtra("result");
                    if (!string.IsNullOrEmpty(result))
                    {
                        long codpess = TryParseLong(result);

                        try
                        {
                            AlertDialog.Builder builder = new AlertDialog.Builder(this);
                            builder.SetTitle("Aviso do Sistema");
                            builder.SetMessage("Tem certeza que deseja excluir este registro ?");

                            builder.SetPositiveButton("OK", (s, e) =>
                            {
                                if (new PessoaController().DeleteByCODPESS(codpess))
                                    this.Msg("EXCLUÍDO COM SUCESSO!");
                                else
                                    this.Msg("ERRO AO EXCLUÍR! VERIFIQUE.");
                            });

                            builder.SetNegativeButton("CANCELAR", (s, e) =>
                            {
                                return;
                            });

                            AlertDialog alertDialog = builder.Create();
                            alertDialog.Show();
                        }
                        catch (Exception ex)
                        {
                            GetError(ex.ToString());
                        }
                    }
                }
        }

        public void LoadDataFromId(long? id)
        {
            Pessoa p = null;
            if (id != null)
                if ((p = new PessoaController().FindById(id.Value)) != null)
                {
                    this.ID = null;
                    txNOMPESS.Text = string.Empty;
                    txNOMFANTA.Text = string.Empty;
                    txIDTPESS.Text = string.Empty;
                    txNROINEST.Text = string.Empty;
                    txDSCENDER.Text = string.Empty;
                    txNROENDER.Text = string.Empty;
                    txCPLENDER.Text = string.Empty;
                    txNOMBAIRR.Text = string.Empty;
                    txNROCEP.Text = string.Empty;
                    txCODMUNIC.Text = string.Empty;
                    txNOMMUNIC.Text = string.Empty;
                    txNROCELUL.Text = string.Empty;
                    txNROFONEC.Text = string.Empty;

                    this.ID = p.ID;
                    txNOMPESS.Text = p.NOMPESS;
                    txNOMFANTA.Text = p.NOMFANTA;
                    txIDTPESS.Text = p.IDTPESS;
                    txNROINEST.Text = p.NROINEST;
                    txDSCENDER.Text = p.DSCENDER;
                    txNROENDER.Text = p.NROENDER;
                    txCPLENDER.Text = p.CPLENDER;
                    txNOMBAIRR.Text = p.NOMBAIRR;
                    txNROCEP.Text = p.NROCEP == 0 ? "" : p.NROCEP.ToString();
                    txCODMUNIC.Text = p.CODMUNIC.ToString();
                    txNROCELUL.Text = p.NROCELUL;
                    txNROFONEC.Text = p.NROFONEC;

                    Municipio munic = null;
                    if ((munic = new MunicipioController().FindById(p.CODMUNIC)) != null)
                        txNOMMUNIC.Text = munic.NOMMUNIC;

                    if (p.IDTDCPES == 0)
                    {
                        radioGroup.Check(Resource.Id.rbCNPJ);
                        btnConsultaCNPJ.Enabled = true;
                    }
                    else if (p.IDTDCPES == 1)
                    {
                        radioGroup.Check(Resource.Id.rbCPF);
                        btnConsultaCNPJ.Enabled = false;
                    }
                    else if (p.IDTDCPES == 2)
                    {
                        radioGroup.Check(Resource.Id.rbOUTRO);
                        btnConsultaCNPJ.Enabled = false;
                    }

                    formatIDTPESS(txIDTPESS.Text);
                }
                else
                    LimparTela();
        }

        public void LoadDataFromCODPESS(long? pCODPESS)
        {
            if (pCODPESS != null)
                if (pCODPESS > 0)
                {
                    Pessoa p = new PessoaController().FindByCODPESS(pCODPESS.Value);
                    if (p != null)
                    {
                        this.ID = p.ID;
                        LoadDataFromId(p.ID);
                    }
                }
        }

        private void GetError(string message)
        {
            string error = "";
            Log.Error(error, message);
            this.Msg(message);
        }

        private long TryParseLong(string value)
        {
            return long.TryParse(RemoveMasks.RemoveMasksToString(value), out long aux) ? aux : 0;
        }

        /// <summary>
        ///  Valida o campo de inscrição estadual
        /// </summary>
        /// <returns></returns>
        private bool ValidateInEst(string escricaoEstadual)
        {
            return (long.TryParse(escricaoEstadual, out long result) && escricaoEstadual.Length == 10);
        }

        /// <summary>
        ///  Limpa a tela com código inteligente
        /// </summary>
        private void LimparTela()
        {
            try
            {
                this.ID = null;

                for (int i = 0; i < RLayout.ChildCount; i++)
                    if (RLayout.GetChildAt(i).GetType() == typeof(TextInputEditText))
                    {
                        var child = (TextInputEditText)RLayout.GetChildAt(i);
                        child.Text = string.Empty;
                    }

                if (this.CurrentFocus != null)
                    CurrentFocus.ClearFocus();

                txCODPESS.Text = string.Empty;
                txNOMPESS.Text = string.Empty;
                txNOMFANTA.Text = string.Empty;
                txIDTPESS.Text = string.Empty;
                txNROINEST.Text = string.Empty;
                txDSCENDER.Text = string.Empty;
                txNROENDER.Text = string.Empty;
                txCPLENDER.Text = string.Empty;
                txNOMBAIRR.Text = string.Empty;
                txNROCEP.Text = string.Empty;
                txCODMUNIC.Text = string.Empty;
                txNOMMUNIC.Text = string.Empty;
                txNROCELUL.Text = string.Empty;
                txNROFONEC.Text = string.Empty;

                txCODPESS.RequestFocus();
            }
            catch (Exception ex)
            {
                GetError(ex.ToString());
            }
        }
        private void LimparFocus()
        {
            if (this.CurrentFocus != null)
                CurrentFocus.ClearFocus();
        }

        private void consultarCPNJ(string cnpj)
        {
            try
            {
                //string URL = "https://www.receitaws.com.br/v1/cnpj/{0}";

                //string newURL = string.Format(URL, cnpj);

                //WebClient wc = new WebClient();
                //string src = wc.DownloadString(newURL);

                //string[] data = src.Split(",");

                //string nomepess = src.Split("\"nome\":\"")[1];
                //string nrocep = Utils.RemoveMasks.RemoveMasksToString(src.Split("\"cep\":\"")[1]);
                //string end = src.Split("\"logradouro\":\"")[1];
                //string nro = src.Split("\"numero\":\"")[1];
                //string bairro = src.Split("\"bairro\":\"")[1];
                //string cidade = src.Split("\"municipio\":\"")[1];
                //string NOMMUNIC = cidade.Split("\",")[0];
                //long? CODMUNIC = new MunicipioController().FindByName(NOMMUNIC).FirstOrDefault().CODMUNIC;


                //txNOMPESS.Text = nomepess.Split("\",")[0];
                //txNROCEP.Text = nrocep.Substring(0, 10);
                //txDSCENDER.Text = end.Split("\",")[0];
                //txNROENDER.Text = nro.Split("\",")[0];
                //txNOMBAIRR.Text = bairro.Split("\",")[0];
                //txCODMUNIC.Text = CODMUNIC.ToString();

                Municipio municipio = new MunicipioController().FindById(new EmpresaController().GetEmpresa().CODMUNIC);
                DNS dns = new ConfigController().GetDNS();

                string str = $"CONSULTARCNPJ{cnpj}{municipio.CODUF}";
                Pessoa pessoa = new PessoaController().ConsultarCNPJ(str, dns.Host, dns.Port);

                if (pessoa != null)
                {
                    txNOMPESS.Text = pessoa.NOMPESS;
                    txNOMFANTA.Text = pessoa.NOMFANTA;
                    txNROINEST.Text = pessoa.NROINEST;
                    txNROCEP.Text = pessoa.NROCEP.ToString();
                    txDSCENDER.Text = pessoa.DSCENDER;
                    txNROENDER.Text = pessoa.NROENDER;
                    txNOMBAIRR.Text = pessoa.NOMBAIRR;

                    if (new MunicipioController().FindByCODMUNGV(pessoa.CODMUNIC) != null)
                        txCODMUNIC.Text = new MunicipioController().FindByCODMUNGV(pessoa.CODMUNIC).CODMUNIC.ToString();
                }
                else
                    this.Msg("ERRO AO BUSCAR CNPJ !");
            }
            catch (Exception e)
            {
                this.Msg("ERRO AO BUSCAR CNPJ !");
            }
        }
    }

}