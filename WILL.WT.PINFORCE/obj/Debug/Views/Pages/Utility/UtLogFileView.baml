   M S B A M L   `   `   `  ����N  JWILL.WT.PINFORCE, Version=0.0.4347.1, Culture=neutral, PublicKeyToken=null[2clr-namespace:WILL.WT.PINFORCE.Views.Pages.Utility$WILL.WT.PINFORCE.Views.Pages.Utility  5      e7clr-namespace:WILL.WT.PINFORCE.ViewModels.Pages.Utility)WILL.WT.PINFORCE.ViewModels.Pages.Utility  5      G CTS.FW.Wpf.v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2TS.FW.Wpf.v2.Controls 5      8    2WILL.WT.PINFORCE.Views.Pages.Utility.UtLogFileView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Dlocal2clr-namespace:WILL.WT.PINFORCE.Views.Pages.Utility     5      Fvm7clr-namespace:WILL.WT.PINFORCE.ViewModels.Pages.Utility     5      Htc9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2     5         xPageSub#��[  5
      .��5      �   � RowDefinitions  5   
   �� 5      $��40 �6   �� 5      $��40 �6   �� 5       � ColumnDefinitions 5   
   �� 5      $	��300 �6   �� 5      .+�5   
   '  !TS.FW.Wpf.v2.Controls.TextBlock3D    
xTextTitle#��[ 6      Text$ LOG LIST��6=   � 5   
   $��1��6    5      �� 5      $��* �6#   �� 5      $��30 �6#   �� 5      $��* �6#   �� 5      $	��320 �6#   .+�5       �� $��1��6   $��~��6(   $��25(�61   $
��Bold"�6?   $��Center=�6Q   $��Center=�6n   )  #TS.FW.Wpf.v2.Controls.DatePickerBtn  5"      $��0��6      DatePickerFormat$
 Date=�6/   (  "System.Windows.Controls.DatePicker   DisplayDate 6G   �� *	StartTime+  5#      $��2��6   $
 Date=�6/    6G   �� *	EndTime+$  TS.FW.Wpf.v2.Controls.EventBtn  5$      $��4��6     	xEventBtn#��[ 6*   $��SEARCH��6M   5%        5'   
   $��1��6   #��[ 6#   $
��SAVE��6F   � 5)   
   $��1��6   $��1��6    5*      �� 5+      �� 5,      $	��320 �6#   .+�5/      $  TS.FW.Wpf.v2.Controls.ValueBtn    	xValueBtn#��[ 6   ��6=   �� *
FileName+  50      $��1��6   #��[ 6*   $��FILTER��6M   51      �� 53   
   $��2��6     xGroup3D#��[ 6   ._�54      �� $��Disabled=�6   $
��Auto=�6D   $��2q�6g   .��55      �� ��6   �� *	LogList+��56      �� 57      .R�58      	� $��50��6*   $��2q�66    	� 	GroupName$	 LOG��6A   ��6Q   ��  �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType$ local:UtLogFileViewL�	 �� Path$"	 DataContext.OnLogSelectedCmd���6�   �� ��59   "   �� 5:   &   
 �� 
TargetType"
 	�66   .R�5;   *   �� #��[ 61   ._�5<   .   � $��2q�63   $��12(�6>   $
��Bold"�6V   ��6r   �� *	IsChecked+  	xColorCvt �� 	Converter# [  �� ConverterParameter$
 MENU��  5=   2   �� 5>   6   �� 5?   6   .+�5B   2   �� $��0��6<   ��6I   �� *Name+�� 5C   2   $��1��6<   ��6I   �� *Time+5D   /   5E   +   5F   '   5H      5I      5K      5L      5M      �� 5O   
   $��2��6   $��1��6   #��[ 6.   ._�5P      �� 
������ 
�������$��0q�6B   $	��2 5q�6V    �� TextWrapping$
 Wrap=�6c    �� 
IsReadOnly .  � Cursor$ Arrow��6�   $
��Auto=�6�   ��6�   �� *LogMsg+5Q      5R      5S      