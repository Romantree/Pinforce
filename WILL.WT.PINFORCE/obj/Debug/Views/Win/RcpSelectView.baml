   M S B A M L   `   `   `  ����N  JWILL.WT.PINFORCE, Version=0.0.4347.1, Culture=neutral, PublicKeyToken=nullG(clr-namespace:WILL.WT.PINFORCE.Views.WinWILL.WT.PINFORCE.Views.Win  5   	   G CTS.FW.Wpf.v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2TS.FW.Wpf.v2.Controls 5   	   .    (WILL.WT.PINFORCE.Views.Win.RcpSelectView   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:WILL.WT.PINFORCE.Views.Win     5   	   Htc9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2     5   	      xPopupWindow#��[  5	   	   $	��650��6/   $	��500��6<   
�������$��15(�6Z   .��5
      �   � RowDefinitions  5   
   �� 5      $��50 �6   �� 5      �� 5      $��50 �6   .+�5   
   '  !TS.FW.Wpf.v2.Controls.TextBlock3D  -   5          
xTextTitle#��[ 5       �� ContentStringFormat$ {0} Recipe List��6T      Text 6=   �� *Title+6|   �� 5   
   $��2��6     xGroup3D#��[ 6   {�6@   �� *
IsEnable+._�5      �� $��2��6    �� Orientation$ 
Horizontal=�6&   $��Right=�6?   .+�5      $  TS.FW.Wpf.v2.Controls.EventBtn    	xEventBtn#��[ 6   $	��150��6A   
��� � �$��OK��6`     5      #��[ 6   $	��150��6A   
���  ��$��CANCEL��6^   5      5      �� 5   
   $��1��6   #��[ 6   ._�5      �� $
��Auto=�6   .��5      �� ��6   �� *	RcpList+��5      �� 5      .R�5      	� $��60��6*    	� 	GroupName$ MAIN_RCP��66   $
��Bold"�6   ��6K   ��  d� 	IsChecked 6_   �� *	IsSelcted+��5    *   ��  �� RelativeSource ��  �� Mode$
 Self=� �� Path$ 	IsChecked�  	xColorCvt	 �� 	Converter#	 [ 
 �� ConverterParameter$

 MENU����5!   *   ��  �� $ FindAncestor=� �� AncestorType.   (WILL.WT.PINFORCE.Views.Win.RcpSelectView"  $" DataContext.OnRcpSelectedCmd���5"   *   �� ��5#   "   �� 5$   &    �� 
TargetType" 	�66   .R�5%   *   ��   xTitleColor#��[ 61   #��z��6[   $��1q�6�    �� CornerRadius$ 5��6�   $��2q�6�   ._�5&   .   �   5'   2   �� 5(   6   �� 5)   6    � ColumnDefinitions 5+   2   �� 5,   6   �� 5-   6   .+�50   2   �� $��2��6C   $
��Left=�6p   $��Center=�6�   $��5 0 0 0q�6�   $��16(�6�   ��6W   �� *Name+6�   �� 51   2   $��1��6C   $��0��6P    �� ContentStringFormat$ 
생성:{0}��6�   $
��Left=�6�   $��Center=�6�   $��5 0 0 0q�6�   $��12(�6�   ��6`   �� *CreationTime+6�   �� 52   2   $��1��6C   $��1��6P   $ 
수정:{0}��6�   $
��Left=�6�   $��Center=�6�   $��5 0 0 0q�6�   $��12(�6�   ��6`   �� *LastWriteTime+6�   53   /   54   +   55   '   57      58      5:      5;      5<      5=      5>      