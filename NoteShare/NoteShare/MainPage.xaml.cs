﻿using Plugin.Media;
using Plugin.Media.Abstractions;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.ListView.XForms.Control.Helpers;
using NoteShare.Database;

namespace NoteShare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public class Name {
            public string name {
                get;
                set;
            }
        }


        //Creatin a collection for all notes (list doesnt update list automatically)
        ObservableCollection<Note> notes = new ObservableCollection<Note>();
        ImageSource currentImage;
        string imageLocation;
        NoteDataAccess noteDB = new NoteDataAccess();


        public MainPage()
        {
            InitializeComponent();
            Title = "NoteShare";
            NavigationPage.SetHasNavigationBar(this, false);
            this.BarBackgroundColor = Color.FromHex("33566D");

            //Updating the list
            MyMenuAsync();
        }


        public void MyMenuAsync()
        {
            
            notes = noteDB.Notes;
            noteMenu.ItemsSource = notes;
        }


        private void SfListVIew_OnTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            
            //if (e.ItemData != null)
            //{
            //    //Reading the elementvalues from the selected list item:
            //    Note tappedItem = e.ItemData as Note;
            //    //bool isContentVisible = tappedItem.Content_isVisible;
            //    int elementId = Convert.ToInt32(tappedItem.NoteId);
            //    var currentIndex = noteMenu.DataSource.DisplayItems.IndexOf(tappedItem);


            //    //edit table isVisible value

            //    if (isContentVisible)
            //    {
            //        DisplayAlert("SfListVIew_OnTapped", isContentVisible.ToString(), "OK");
                    
            //        isContentVisible = false;
                    
            //    }
            //    else
            //    {
            //        DisplayAlert("SfListVIew_OnTapped", isContentVisible.ToString(), "OK");
            //        isContentVisible = true;



            //        //Only updating the notes list, not the actual local database. that means it will reset the next time the app loads.
            //        //TODO: Add functionality that edits database instead. Then people can make certain notes be open by default 
            //        foreach (Notes updateVisibility in notes)
            //        {
            //            if (updateVisibility.NoteId == elementId)
            //            {
            //                updateVisibility.Content_isVisible = true;
                            
            //            }
            //        }
                   



            //    }
                
            //}
        }
        
        #region Tab 2
        private void Btn_newNote(object sender, ClickedEventArgs e)
        {
            String title = entry_Title.Text;
            String description = entry_Description.Text;

            Note tempNote = new Note { Title = title, Description = description, Content = "alal lalal lalal all alla lalla lalal lalal alll content", Icon = imageLocation, Content_isVisible = false};
            noteDB.AddNewNote(tempNote);

            // Updating ListView
            MyMenuAsync(); 

            DisplayAlert("Congrats", "The note has been created", "Ok");
        }


        private void clearDatabase(object sender, EventArgs e)
        {
            //Dropping the noteDB table
            noteDB.DropTable();

            //Updating the listView
            MyMenuAsync();
        }

        private void Entry_Title_Focused(object sender, FocusEventArgs e)
        {
            entry_Title.Text = null;
        }

        private void Entry_Description_Focused(object sender, FocusEventArgs e)
        {
            entry_Description.Text = null;
        }

        private void img_tapped_upload(object sender, EventArgs e)
        {
            DisplayAlert("Expand", "", "OK");


            // btn_uploadIconAsync(); then iconPreview.Source = icon
        }
        #endregion



        #region Xamaring Plugin Media
        private async void Btn_takePicIconAsync(object sender, EventArgs e)
        {

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "NoteShare_icon",
                SaveToAlbum = true,
                CompressionQuality = 15,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = CameraDevice.Rear
            });

            if (file == null)
                return;


            await DisplayAlert("File Location", file.Path, "OK");
            imageLocation = file.Path;

            currentImage = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;

            });
            iconPreview.Source = currentImage;


        }


        private async void Btn_uploadIconAsync(object sender, EventArgs e)
        {

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                CompressionQuality = 15,
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,

            });


            if (file == null)
                return;

            imageLocation = file.Path;
            currentImage = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
            iconPreview.Source = currentImage;

        }
    }
    #endregion
}