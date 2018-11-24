using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio;
using NAudio.Wave;

public static class SoundDriver
{
    private static MixingWaveProvider32 mixer;
    private static AsioOut waveOut;
    private static WaveFormat waveformat;
    private static Dictionary<string, IWaveProvider> DctWaveProvider;

    public static List<string> GetAsioDriverNames()
    {
        return AsioOut.GetDriverNames().ToList();
    }

    public static void Init(string drivername)
    {
        mixer = new MixingWaveProvider32();

        waveOut = new AsioOut(drivername);//NAudio.CoreAudioApi.AudioClientShareMode.Exclusive,100);
        //waveOut = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Shared, 100);
        waveOut.Init(mixer);
        waveformat = new WaveFormat(44100, 2);

        DctWaveProvider = new Dictionary<string, IWaveProvider>();
    }

    public static void Init(string drivername, int channeloffset)
    {
        mixer = new MixingWaveProvider32();

        waveOut = new AsioOut(drivername);
        waveOut.ChannelOffset = channeloffset;
        waveOut.Init(mixer);
        waveformat = new WaveFormat(44100, 2);

        DctWaveProvider = new Dictionary<string, IWaveProvider>();
    }

    public static void Play()
    {
        waveOut.Play();
    }
    public static PlaybackState PlaybackState()
    {
        return waveOut.PlaybackState;
    }

    public static void AddWaveProvider(IWaveProvider waveprovider, string name)
    {

        //Wave16ToFloatProviderとか使ってIeeeFloatにしないとだめ
        if (waveprovider.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
            return;
        if (waveprovider.WaveFormat.SampleRate != waveformat.SampleRate
            || waveprovider.WaveFormat.Channels != waveformat.Channels)
            return;
        if (DctWaveProvider.ContainsKey(name))
        {
            RemoveWaveProvider(name);
        }
        DctWaveProvider.Add(name, waveprovider);
        mixer.AddInputStream(waveprovider);
    }

    public static void RemoveWaveProvider(IWaveProvider waveprovider)
    {
        mixer.RemoveInputStream(waveprovider);
    }

    public static void RemoveWaveProvider(string name)
    {
        if (DctWaveProvider.ContainsKey(name))
        {
            mixer.RemoveInputStream(DctWaveProvider[name]);
            DctWaveProvider.Remove(name);
        }
    }

    public static void Dispose()
    {
        waveOut.Stop();
    }

    public static IWaveProvider GetWaveProvider(string name)
    {
        if (DctWaveProvider.ContainsKey(name))
        {
            return DctWaveProvider[name];
        }
        else
        {
            return null;
        }
    }
}
