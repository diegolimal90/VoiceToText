using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using System.Globalization;
using Microsoft.Speech.Recognition.SrgsGrammar;

namespace VoiceToText
{
    public partial class Form1 : Form
    {
        static CultureInfo ci = new CultureInfo("pt-BR");

        static SpeechRecognitionEngine reconhecedor;

        SpeechSynthesizer resposta = new SpeechSynthesizer();

        public string[] listaPalavras = { "oi", "quem é você", "como você está" };

        public Form1()
        {
            InitializeComponent();

            Init();
            
        }

        public void Gramatica()
        {
            try
            {
                reconhecedor = new SpeechRecognitionEngine(ci);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            var gramatica = new Choices();
            gramatica.Add(listaPalavras);

            var gb = new GrammarBuilder();
            gb.Append(gramatica);

            try
            {
                var g = new Grammar(gb);

                try
                {
                    reconhecedor.RequestRecognizerUpdate();
                    reconhecedor.LoadGrammarAsync(g);

                    reconhecedor.SpeechRecognized += Sre_Reconhecimento;
                    reconhecedor.SetInputToDefaultAudioDevice();

                    resposta.SetOutputToDefaultAudioDevice();
                    reconhecedor.RecognizeAsync(RecognizeMode.Multiple);
                    reconhecedor.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(audio_Level);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Erro ao criar reconhecedor" + e.Message);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao criar a gramática" + ex.Message);
            }
        }

        private void Reconhecedor_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            resposta.Volume = 100;
            resposta.Rate = 2;
            Gramatica();
        }

        void Sre_Reconhecimento(object sender, SpeechRecognizedEventArgs e)
        {
            string frase = e.Result.Text;

            richTextBox1.Text += frase + " ";
            switch (frase)
            {
                case "oi":
                    resposta.SpeakAsync("Olá, em que posso ajuda-lo!");
                    break;
                case "quem é você":
                    resposta.SpeakAsync("sou sua namorada virtual!");
                    break;
                case "como você está":
                    resposta.SpeakAsync("nossa você é muito fofo, estou bem e você");
                    break;
            }
                
            
            //else
            //{
            //    resposta.SpeakAsync("Estou esperando você falar algo.");
            //}
        }

        void audio_Level(object sender, AudioLevelUpdatedEventArgs e)
        {           
            PBarAudio.Maximum = 100;
            PBarAudio.Value = e.AudioLevel;
        }
    }
}
