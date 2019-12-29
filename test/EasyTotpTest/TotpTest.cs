using System;
using System.Text;
using System.Threading.Tasks;
using EasyTotp;
using NUnit.Framework;

namespace EasyTotpTest
{
    [TestFixture]
    public class TotpTest
    {
        private const string Key = "12345678901234567890123456789012"; //32 chars 
        private readonly byte[] _aesKey = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
        private readonly byte[] _aesIv = Encoding.UTF8.GetBytes("1234567890123456");
        

        [Test]
        public void SameTotpTheSameTimeStepEncrypted()
        {
            while(true){
                if(DateTime.Now.Second%5==0) break;
                else Task.Delay(800).GetAwaiter().GetResult();
            }

            var totp = new Totp()
                    .Secret(Key)
                    .Length(8)
                    .ValidFor(TimeSpan.FromSeconds(5))
                    .UseDefaultEncryptor(_aesKey,_aesIv);

            var value1 = totp.ComputeEncrypted();
            var value1Dec= totp.Decrypt(value1);

            Task.Delay(3000).GetAwaiter().GetResult();

            var value2 = totp.ComputeEncrypted();
            var value2Dec= totp.Decrypt(value2);

            Assert.AreEqual(value1Dec, value2Dec);
        }

        [Test]
        public void NotSameTotp_OutOfTimeStep_Encrypted()
        {
            while(true){
                if(DateTime.Now.Second%5==0) break;
                else Task.Delay(800).GetAwaiter().GetResult();
            }

            var totp = new Totp()
                    .Secret(Key)
                    .Length(8)
                    .ValidFor(TimeSpan.FromSeconds(5))
                    .UseDefaultEncryptor(_aesKey,_aesIv);

            var value1 = totp.ComputeEncrypted();
            var value1Dec= totp.Decrypt(value1);

            Task.Delay(6000).GetAwaiter().GetResult();

            var value2 = totp.ComputeEncrypted();
            var value2Dec= totp.Decrypt(value2);

            Assert.AreNotEqual(value1Dec, value2Dec);
        }
        [Test]
        public void SameTotpTheSameTimeStep()
        {

            while(true){
                if(DateTime.Now.Second%5==0) break;
                else Task.Delay(800).GetAwaiter().GetResult();
            }

            var totp = new Totp()
                    .Secret(Key)
                    .Length(8)
                    .ValidFor(TimeSpan.FromSeconds(5));

            var value1 = totp.Compute();

             Task.Delay(2000).GetAwaiter().GetResult();

            var value2 = totp.Compute();

            Assert.AreEqual(value1, value2);
        }

        [Test]
        public void NotSameTotp_OutOfTimeStep()
        {
            while(true){
                if(DateTime.Now.Second%5==0) break;
                else Task.Delay(800).GetAwaiter().GetResult();
            }
            var totp = new Totp()
                    .Secret(Key)
                    .Length(8)
                    .ValidFor(TimeSpan.FromSeconds(5));

            var value1 = totp.Compute();

             Task.Delay(6000).GetAwaiter().GetResult();

            var value2 = totp.Compute();

            Assert.AreNotEqual(value1, value2);
        }
    }
}