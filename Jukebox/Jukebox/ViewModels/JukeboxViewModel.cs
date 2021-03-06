﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Jukebox.Albums.ViewModels;
using Jukebox.Artists.ViewModels;
using Jukebox.NowPlaying.ViewModels;
using Jukebox.Playlist.ViewModels;
using Jukebox.Shared.Classes;
using Jukebox.Shared.Extensions;
using Jukebox.Shared.ViewModels;
using Jukebox.Songs.ViewModels;
using Jukebox.Utility.ViewModels;
using Jukebox.ViewAlbum.ViewModels;
using Jukebox.ViewArtist.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Windows.Input;

namespace Jukebox.ViewModels
{
    public class JukeboxViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;

        public JukeboxViewModel(
            SongsPaneViewModel songsPaneViewModel,
            AlbumsPaneViewModel albumsPaneViewModel,
            NowPlayingPaneViewModel nowPlayingPaneViewModel,
            UtilityPaneViewModel utilityPaneViewModel,
            ViewAlbumPaneViewModel viewAlbumPaneViewModel,
            ArtistsPaneViewModel artistsPaneViewModel,
            ViewArtistPaneViewModel viewArtistPaneViewModel,
            PlaylistPaneViewModel playlistPaneViewModel,
            IMessenger messenger,
            RecordPlayer player)
        {
            SongsPaneViewModel = songsPaneViewModel ?? throw new ArgumentNullException(nameof(songsPaneViewModel));
            AlbumsPaneViewModel = albumsPaneViewModel ?? throw new ArgumentNullException(nameof(songsPaneViewModel));
            ArtistsPaneViewModel = artistsPaneViewModel ?? throw new ArgumentNullException(nameof(artistsPaneViewModel));
            NowPlayingPaneViewModel = nowPlayingPaneViewModel ?? throw new ArgumentNullException(nameof(nowPlayingPaneViewModel));
            UtilityPaneViewModel = utilityPaneViewModel ?? throw new ArgumentNullException(nameof(utilityPaneViewModel));
            ViewAlbumPaneViewModel = viewAlbumPaneViewModel ?? throw new ArgumentNullException(nameof(viewAlbumPaneViewModel));
            ViewArtistPaneViewModel = viewArtistPaneViewModel ?? throw new ArgumentNullException(nameof(viewArtistPaneViewModel));
            PlaylistPaneViewModel = playlistPaneViewModel ?? throw new ArgumentNullException(nameof(playlistPaneViewModel));

            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            Player = player ?? throw new ArgumentNullException(nameof(player));

            _messenger.RegisterMessageListener<List<SongViewModel>>(this, Messages.SendSongsToViewAlbum, ViewAlbumSongs);
            _messenger.RegisterMessageListener<List<ArtistViewModel>>(this, Messages.AllArtistsLoaded, LoadArtists);
            _messenger.RegisterMessageListener<List<AlbumViewModel>>(this, Messages.ViewArtistAlbums, ViewArtistAlbums);

            SongsButtonCommand = new RelayCommand(SongsButtonCommandMethod);
            AlbumsButtonCommand = new RelayCommand(AlbumsButtonCommandMethod);
            NowPlayingButtonCommand = new RelayCommand(NowPlayingButtonCommandMethod);
            ArtistsButtonCommand = new RelayCommand(ArtistsButtonCommandMethod);
            PlaylistButtonCommand = new RelayCommand(PlaylistButtonCommandMethod);
        }

        #region Properties
        public RecordPlayer Player { get; }

        //public ObservableCollection<SongViewModel> PlayList
        //{
        //    get => _playList;
        //    set => Set(ref _playList, value);
        //}
        //private ObservableCollection<SongViewModel> _playList = new ObservableCollection<SongViewModel>();

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value);
        }
        private int _selectedIndex;
        #endregion

        #region Panes
        public SongsPaneViewModel SongsPaneViewModel { get; }
        public AlbumsPaneViewModel AlbumsPaneViewModel { get; }
        public ArtistsPaneViewModel ArtistsPaneViewModel { get; set; }
        public NowPlayingPaneViewModel NowPlayingPaneViewModel { get; }
        public UtilityPaneViewModel UtilityPaneViewModel { get; }
        public ViewAlbumPaneViewModel ViewAlbumPaneViewModel { get; }
        public ViewArtistPaneViewModel ViewArtistPaneViewModel { get; }
        public PlaylistPaneViewModel PlaylistPaneViewModel { get; }
        #endregion

        #region Commands
        public ICommand SongsButtonCommand { get; set; }
        public ICommand AlbumsButtonCommand { get; set; }
        public ICommand ArtistsButtonCommand { get; set; }
        public ICommand PlaylistButtonCommand { get; set; }
        public ICommand NowPlayingButtonCommand { get; set; }

        #endregion

        #region Command Methods
        private void SongsButtonCommandMethod() => SelectedIndex = 0;
        private void AlbumsButtonCommandMethod() => SelectedIndex = 1;
        private void ArtistsButtonCommandMethod() => SelectedIndex = 2;
        private void PlaylistButtonCommandMethod() => SelectedIndex = 3;
        private void NowPlayingButtonCommandMethod() => SelectedIndex = 4;
        #endregion

        #region Message Methods
        private void ViewAlbumSongs(NotificationMessage<List<SongViewModel>> message)
        {
            ViewAlbumPaneViewModel.Songs.Clear();

            var sortedSongs = message.Content.OrderBy(x => x.TrackNumber).ToList();

            foreach (var song in sortedSongs)
            {
                ViewAlbumPaneViewModel.Songs.Add(song);
            }

            SelectedIndex = 5;
        }

        private void ViewArtistAlbums(NotificationMessage<List<AlbumViewModel>> message)
        {
            ViewArtistPaneViewModel.Albums.Clear();

            var sortedAlbums = message.Content.OrderBy(x => x.AlbumYear).ToList();

            foreach (var album in sortedAlbums)
            {
                ViewArtistPaneViewModel.Albums.Add(album);
            }

            SelectedIndex = 6;
        }

        private void LoadArtists(NotificationMessage<List<ArtistViewModel>> message)
        {
            ArtistsPaneViewModel.Artists.Clear();

            var sortedArtists = message.Content.OrderBy(x => x.Artist);

            foreach(var artist in sortedArtists)
            {
                ArtistsPaneViewModel.Artists.Add(artist);
            }
        }
        #endregion
    }
}
