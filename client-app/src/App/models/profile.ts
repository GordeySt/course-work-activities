export interface IProfile {
  displayedName: string;
  userName: string;
  bio: string;
  mainImage: string;
  followersCount: number;
  followingsCount: number;
  isFollowing: boolean;
  photos: IPhoto[];
}

export interface IPhoto {
  id: string;
  url: string;
  isMain: boolean;
}

export interface IUserActivity {
  id: string;
  title: string;
  category: string;
  date: Date;
}
