   M S B A M L   `   `   `  ����N  JWILL.WT.PINFORCE, Version=0.0.4347.1, Culture=neutral, PublicKeyToken=null?$clr-namespace:WILL.WT.PINFORCE.ViewsWILL.WT.PINFORCE.Views  5   	   G CTS.FW.Wpf.v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ8clr-namespace:TS.FW.Wpf.v2.Helpers;assembly=TS.FW.Wpf.v2TS.FW.Wpf.v2.Helpers 5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089M4clr-namespace:System.Globalization;assembly=mscorlibSystem.Globalization 5   	   I)clr-namespace:WILL.WT.PINFORCE.ViewModelsWILL.WT.PINFORCE.ViewModels  5	   	   K GTS.FW.Wpf, Version=1.0.9053.17879, Culture=neutral, PublicKeyToken=nullJ3clr-namespace:TS.FW.Wpf.Controls;assembly=TS.FW.WpfTS.FW.Wpf.Controls 5
   	   S9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2TS.FW.Wpf.v2.Controls 5   	   %    WILL.WT.PINFORCE.Views.MainView   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:WILL.WT.PINFORCE.Views     5   	   Khelper8clr-namespace:TS.FW.Wpf.v2.Helpers;assembly=TS.FW.Wpf.v2     5   	   Eglob4clr-namespace:System.Globalization;assembly=mscorlib     5   	   8vm)clr-namespace:WILL.WT.PINFORCE.ViewModels     5	   	   Bts3clr-namespace:TS.FW.Wpf.Controls;assembly=TS.FW.Wpf     5
   	   Htc9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2     5   	      	xMainView#��[  5   	   .��5      (�   (� Stretch$
  Fill=�6   .��5   
   � $
��1280��6   $
��1024��6    � RowDefinitions 5      �� 5      $��80 �6    �� 5      �� 5      $��80 �6    .+�5      �� $��0��6     xGroup3D#��[ 6"   ._�5      �  � ColumnDefinitions 5      �� 5      $	��150 �6+   �� 5      �� 5      $	��380 �6+   .+�5!      ��   	xBorder3D#��[ 6     xLogBackColor#��[ 6@   $	��2 5q�6l   ._�5"      � .+�5#      �� -   5        $��../Assets/로고.png��5#   $   $��Uniform=�6>   $��5q�6P   6[   �� 5$      $
��Bold"�5%   (   
���iii�$��10(�6O   $��Right=�5&   (   $��Bottom=�6D   $
��10 3q�6_   ��5$   (   �� (  "TS.FW.Wpf.v2.Helpers.ProgramHelper
   Ins �� Source# Z  �� Path$ Memory� �� StringFormat$ 	{0:F1} MB��5&   _   5'      5(      � 5*      $��2��6    5+      �� 5,      �� 5-      $��40 �6/   �� 5.      $	��120 �6/   .+�51      �� $��2��6!   #��[ 61   
���iii�$	��2 4q�6i   ._�52      �� $
��Bold"�6)   
�������$��Center=�6b   .+�53   "   �� $��Center=�6,   $��12(�6I   ��6W   �� )  #TS.FW.Wpf.v2.Helpers.DateTimeHelper
   Ins# Z $	 Now�$ {0:d}�� �� InstalledUICulture	 �� ConverterCulture#	 Z �� 54   "   $��Center=�6,   $��15(�6I   ��6W   �� # Z $	 Now�$ {0:tt hh:mm:ss}��#	 Z �� 55   "   $��Center=�6,   $��10(�6I   $��0 5 0 0q�6�   ��6W   �� # Z $ Version�#	 Z 6�   56      57      � 59      $��1��6   $	��0 2q�6/   
 � DataContext
 6<   �� *	TowerLamp+ 5:      �� 5;   "   �� 5<   "   �� 5=   "   .+�5@      !  TS.FW.Wpf.Controls.Border3D  $��0��6*   $��2q�67      CornerRadius$ 4��6B   ��6S   �� *Red+  xOnOffColor �� 	Converter# [  �� ConverterParameter$	 Red��  5A      $��1��6*   $��2q�67   $ 4��6B   ��6S   �� *Yellow+# [ $ Yellow��  5B      $��2��6*   $��2q�67   $ 4��6B   ��6S   �� *Green+# [ $ Green��5C      � 5E       5F      �� 5G   "   �� 5H   "   �� 5I   "   .+�5M      $  TS.FW.Wpf.v2.Controls.EventBtn  $��1��6*     xIconBtn#��[ 6:     appbar_warning   Icon# [ 6\   $��Alarm��6�   $��Alarm��6�   ��5N   *   �� *Alarm+  	xColorCvt# [ $ Alarm��{�6�   �� *
IsEnable+  5O      $��1��6*   #��[ 6:     appbar_uservoice# [ 6\   $��Buzzer��6�   $��Buzzer��6�   $y�	Collapsed=�5P   �   ��6*   �� *Buzzer+# [ $
 Lamp��6�     5Q      $��2��6*   #��[ 6:    	 appbar_people_multiple# [	 6\   $��10(�6�   $��Login��6�   ��6�   �� *Login+6�   5R      5S      � 5U      $��1��6    5V      �� 5W      $
��auto �6/   �� 5X      .+�5\      �� #��[ 6!   
���iii�$	��2 5q�6Y   ._�5_      � $
��20 5q�6#    5`   "   �� 5a   &   �� 5b   &   �� 5c   &   �� 5d   &    5f   "   �� 5g   &   �� 5h   &   .+�5k   "   �� $��0��6,   $��0��6<   $��4��6I   $	��400��6]   $!��Pin Force Evaluating System��6i   
�������$
��Bold"�6�   $
��Left=�6�   $��Center=�6�   $��20(�6�   �� 5l   "   $��0��6,   $��1��6<   $��Loadcell #1��6I   
�������$
��Bold"�6t   $
��Left=�6�   $��Center=�6�   $��14(�6�   �� 5m   "   $��2��6,   $��1��6<   $��Loadcell #2��6I   
�������$
��Bold"�6t   $
��Left=�6�   $��Center=�6�   $��14(�6�     5o   "   $��1��6.   $��1��6>   $	��5 5q�6K   $��80��6X   $ 10��6c   $
��Left=�6�   ��6u   �� *LoadcellModel_1.IsOpen+# [ $ Green��6�     5p   "   $��3��6.   $��1��6>   $	��5 5q�6K   $��80��6X   $ 10��6c   $
��Left=�6�   ��6u   �� *LoadcellModel_2.IsOpen+# [ $ Green��6�   5q      5�      5�      5�      5�      �� 5�      $��1��6   #��[ 6"   {�6E   �� *
IsEnable+._�5�      $  TS.FW.Wpf.v2.Controls.MenuView  ��6   �� *
MenuList+5�      �� 5�      $��2��6   #��[ 6"   ._�5�      �  5�      �� 5�      �� 5�      $��90 �6+   .+�5�      $  TS.FW.Wpf.v2.Controls.MenuList     Orientation$ 
Horizontal=�6i    
 	xMainMenu   	MenuStyle# [
 6�   ��6"   �� *
MenuList+   SelectedMenu 6C   �� *SelectedMenu+{�5�   "   �� *
IsEnable+  5�      $��1��6"   #��[ 62   $
��EXIT��6T     appbar_power# [ 6c      	DelayTime$ 1Q�6�   $��5q�6�   5�      5�      5�      5�      5�      