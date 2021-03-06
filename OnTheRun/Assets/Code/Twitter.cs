﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Twitter : MonoBehaviour {

    public GameObject BannerText;

    private List<string> idsDone = new List<string>();
    private List<string> commandsToExecute = new List<string>();

    private DateTime startOfGame;
    private bool isExecuting = false;

    private AudioSource aud;

    void Start() {
        startOfGame = DateTime.Now;
        StartCoroutine( Search() );
        aud = GetComponent<AudioSource>();
        StartCoroutine( AddDifficulty() );
    }

    private IEnumerator AddDifficulty() {
        while ( true ) {
            yield return new WaitForSeconds( 15 );

            if ( commandsToExecute.Count == 0 ) {
                int val = UnityEngine.Random.Range( 0, 6 );
                switch ( val ) {
                    case 0:
                        commandsToExecute.Add( "invert" );
                        break;
                    case 1:
                        commandsToExecute.Add( "shake" );
                        break;
                    case 2:
                        commandsToExecute.Add( "invisible" );
                        break;
                    case 3:
                        commandsToExecute.Add( "fisheye" );
                        break;
                    case 4:
                        commandsToExecute.Add( "slow" );
                        break;
                    case 5:
                        commandsToExecute.Add( "reddit" );
                        break;
                }
            }

        }
    }

    // Update is called once per frame
    void Update() {
        if ( !isExecuting && commandsToExecute.Count > 0 && !Tuna.Caught ) {
            isExecuting = true;
            var cmd = commandsToExecute[0];
            commandsToExecute.RemoveAt( 0 );
            StartCoroutine( ExecuteCmd( cmd ) );
        }
    }

    private IEnumerator ExecuteCmd( string input ) {
        if ( Tuna.Caught ) {
            yield break;
        }
        var cmd = input;
        if ( input.Contains( " " ) ) {
            var index = input.IndexOf( " " );
            cmd = input.Substring( 0, index ).Trim();
            var msg = input.Remove( 0, index ).Trim();
            StartCoroutine( ShowMessage( string.Format( "#{0}: {1}", cmd, msg ) ) );
        } else {
            StartCoroutine( ShowMessage( "#" + cmd ) );
        }
        Debug.Log( "Starting " + cmd );

        switch ( cmd ) {
            case "invert":
                Player.Invert = true;
                yield return new WaitForSeconds( 5f );
                Player.Invert = false;
                break;
            case "shake":
                iTween.ShakePosition( Camera.main.gameObject, new Vector3( 1.5f, 1.5f ), 3f );
                yield return new WaitForSeconds( 5f );
                break;
            case "invisible":
                Tuna.Invisible = true;
                yield return new WaitForSeconds( 5f );
                Tuna.Invisible = false;
                break;
            case "slow":
                Player.Slow = true;
                yield return new WaitForSeconds( 5f );
                Player.Slow = false;
                break;
            case "fisheye":
                UnityStandardAssets.ImageEffects.Fisheye.Active = true;
                yield return new WaitForSeconds( 5f );
                UnityStandardAssets.ImageEffects.Fisheye.Active = false;
                break;
            case "reddit":
                Shroomer.Activate();
                yield return new WaitForSeconds( 5f );
                break;
        }

        Debug.Log( "Done with " + cmd );

        yield return new WaitForSeconds( 1 );
        isExecuting = false;
        yield break;
    }

    private IEnumerator ShowMessage( string message ) {
        iTween.Stop( BannerText );
        yield return new WaitForSeconds( 0.25f );
        aud.Play();
        BannerText.transform.localPosition = new Vector3( 500, 200 );
        BannerText.GetComponent<Text>().text = message;
        iTween.MoveBy( BannerText, new Vector3( -7500, 0 ), 100 );
    }

    private IEnumerator Search() {
        Debug.Log( "Searching Twitter" );
        var www = new WWW( "https://twitter.com/search?f=tweets&vertical=default&q=%23coconuts2016&src=typd" );
        yield return www;
        var text = www.text;
        var index = text.IndexOf( "<div id=\"timeline\"" );
        text = text.Remove( 0, index );
        index = text.IndexOf( "<div class=\"stream-footer" );
        text = text.Substring( 0, index );
        index = text.IndexOf( "<li class=\"js-stream-item" );
        text = text.Remove( 0, index );

        var tweets = text.Split( new string[] { "<li class=\"js-stream-item" }, StringSplitOptions.RemoveEmptyEntries );

        var cmds = new List<string>();
        foreach ( var item in tweets ) {
            var id = item.Remove( 0, item.IndexOf( "data-item-id" ) + 13 );
            id = id.Substring( 0, id.IndexOf( "id" ) );
            id = id.Replace( "\"", "" ).Trim();

            if ( idsDone.Contains( id ) ) continue;

            var tstamp = item.Remove( 0, item.IndexOf( "data-time" ) + 10 );
            tstamp = tstamp.Substring( 0, tstamp.IndexOf( "data-time-ms" ) );
            tstamp = tstamp.Replace( "\"", "" ).Trim();

            var dt = new DateTime( 1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc );
            dt = dt.AddSeconds( double.Parse( tstamp ) );
            dt = dt.AddHours( 1 );
            if ( dt.Ticks < startOfGame.Ticks ) continue;

            var temp = item.Remove( 0, item.IndexOf( "<strong>coconuts2016</strong>" ) );
            temp = temp.Remove( 0, temp.IndexOf( "</a>" ) + 4 );
            temp = temp.Substring( 0, temp.IndexOf( "</p>" ) ).Trim();

            cmds.Add( temp );
            idsDone.Add( id );
        }

        cmds.Reverse();
        commandsToExecute.AddRange( cmds );

        yield return new WaitForSeconds( 5 );
        StartCoroutine( Search() );
    }
}
