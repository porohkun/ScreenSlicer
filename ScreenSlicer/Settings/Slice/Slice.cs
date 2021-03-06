﻿using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace ScreenSlicer
{
    public class Slice : INotifyPropertyChanged, ICloneable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Orientation _orientation;
        private int _position;

        [JsonProperty(nameof(Orientation))]
        public Orientation Orientation
        {
            get => _orientation;
            private set
            {
                if (_orientation != value)
                {
                    _orientation = value;
                    NotifyPropertyChanged(nameof(Orientation));
                }
            }
        }

        [JsonProperty(nameof(Position))]
        public int Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    NotifyPropertyChanged(nameof(Position));
                }
            }
        }

        public Slice() { }

        public Slice(Orientation orientation, int position)
        {
            _orientation = orientation;
            _position = position;
        }

        public object Clone()
        {
            return new Slice(Orientation, Position);
        }

        public bool Bound(Region region)
        {
            var position = Position;
            if (region.RegionA != null && region.RegionB != null)
                switch (Orientation)
                {
                    case Orientation.Vertical:
                        if (region.Bounds.Width < region.MinVerticalSlice * 2)
                            return false;
                        position = Math.Max(position, region.MinVerticalSlice);
                        position = Math.Min(position, region.MaxVerticalSlice);
                        break;
                    case Orientation.Horizontal:
                        if (region.Bounds.Height < region.MinHorizontalSlice * 2)
                            return false;
                        position = Math.Max(position, region.MinHorizontalSlice);
                        position = Math.Min(position, region.MaxHorizontalSlice);
                        break;
                }
            Position = position;
            return true;
        }
    }
}
