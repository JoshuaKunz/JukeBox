﻿using Jukebox.Shared.Models;
using Jukebox.Shared.ViewModels;
using System.Collections.Generic;

namespace Jukebox.Shared.Factory.Interface
{
    public interface IJukeboxFactory
    {
        void GetImageFromMp3(string path, string albumName);

        SongViewModel ConvertSongModel(SongModel model);
        IEnumerable<SongViewModel> ConvertSongModels(IEnumerable<SongModel> models);

        AlbumViewModel ConvertSongsToAlbum(List<SongViewModel> songs);

        List<ArtistViewModel> ConvertAlbumsToArtist(List<AlbumViewModel> albums);
        ArtistViewModel ConvertAlbumsToArtistSingle(List<AlbumViewModel> albums);
    }
}
