   M S B A M L   `   `   `  ����N  JWILL.WT.PINFORCE, Version=0.0.4347.1, Culture=neutral, PublicKeyToken=nullW0clr-namespace:WILL.WT.PINFORCE.Views.Pages.Alarm"WILL.WT.PINFORCE.Views.Pages.Alarm  5      G CTS.FW.Wpf.v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2TS.FW.Wpf.v2.Controls 5      a5clr-namespace:WILL.WT.PINFORCE.ViewModels.Pages.Alarm'WILL.WT.PINFORCE.ViewModels.Pages.Alarm  5      9    3WILL.WT.PINFORCE.Views.Pages.Alarm.AlarmHistoryView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Blocal0clr-namespace:WILL.WT.PINFORCE.Views.Pages.Alarm     5      Htc9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2     5      Dvm5clr-namespace:WILL.WT.PINFORCE.ViewModels.Pages.Alarm     5         xPageSub#��[  5
      .��5      �   � RowDefinitions  5   
   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      .+�5   
   ��  �� Orientation$ 
Horizontal=�6   .+�5      '  !TS.FW.Wpf.v2.Controls.TextBlock3D    
xTextTitle#��[ 6   $	��150��6A      Text$ Search Time��6M   )  #TS.FW.Wpf.v2.Controls.DatePickerBtn  5      $	��200��6      DatePickerFormat$
 Date=�6+   (  "System.Windows.Controls.DatePicker   DisplayDate 6C   �� *	StartTime+�� 5      $��50��6   $��~��6#   $��25(�6,    �� TextAlignment$ Center=�6:   $��Center=�6Q   $��Center=�6n     5      $	��200��6   $
 Date=�6+    6C   �� *	EndTime+$  TS.FW.Wpf.v2.Controls.EventBtn  5      $	��158��6     	xEventBtn#��[ 6&   $��Search��6I   5      �� 5   
   $��1��6   $ 
Horizontal=�6"   .+�5        #��[ 6   $	��150��6A   $
 Page��6M     5        
xTextState#��[ 6   $	��150��6A    6M   �� *Page+  5      $��75��6   #��[ 6%   $��First��6H     5      $	��150��6   #��[ 6&   $
��Prev��6I     5       $	��150��6   #��[ 6&   $
��Next��6I     5!      $��75��6   #��[ 6%   $
��Last��6H   5"        5$   
   $��2��6   #��[ 6&   $ Search Condition��6J   &   System.Windows.Controls.DataGrid  5&   
   $��3��6   ��6    �� *	AlarmList+   Columns 5'      0  *System.Windows.Controls.DataGridTextColumn  5(        GridTextBase1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [ 6%   ,  &System.Windows.Controls.DataGridColumn   Header$ OCCURRENCE TIME��6R   	   Width5	  /System.Windows.Controls.DataGridLengthConverter$		 200	 6k   
   
IsReadOnly
 .    Binding 6�   �� *
PostTime+  5)      # [ 6%   $ 
CLEAR TIME��6R   $		 200	 6f   
 .  6�   �� *	ClearTime+  5*      # [ 6%   $ DESCRIPTION��6R   $	 *	 6g   
 .  6�   �� *Alarm+  5+      # [ 6%   $ LEVEL��6R   $		 150	 6a   
 .  6   �� *Level+5-      5.      5/      