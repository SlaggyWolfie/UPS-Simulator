using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using GXPEngine;

public class TextBox : Canvas
{
    private float _positionX;
    private float _positionY;

    private bool _isDestroyed;
    private Font _hudFont;
    private PrivateFontCollection _fontCollection;

    public TextBox(int pWidth, int pHeight, float pX, float pY, int size = 80) : base(pWidth, pHeight)
    {
        SetText("");
        _isDestroyed = false;

        _positionX = pX;
        _positionY = pY;

        graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
        _fontCollection = new PrivateFontCollection();
        _fontCollection.AddFontFile("assets/menu/font/SpecialElite.ttf");

        _hudFont = new Font(_fontCollection.Families[0], size);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _isDestroyed = true;
    }

    public void SetText(string text, bool clear = true)
    {
        if (_isDestroyed == false)
        {
            if (clear) graphics.Clear(Color.Transparent);
            graphics.DrawString(text, _hudFont, Brushes.White, _positionX, _positionY);
            alpha = 0.9f;
        }
    }
    public void SetText(string text, float pX, float pY, bool clear = true, bool centerX = false, bool centerY = false)
    {
        if (_isDestroyed == false)
        {
            if (clear) graphics.Clear(Color.Transparent);
            //graphics.DrawString(text, _hudFont, (SolidColorBrush)new BrushConvertor().ConvertFromString("#544335"), pX, pY);
            //graphics.DrawString(text, _hudFont, new SolidBrush(Color.FromArgb(84, 67, 53)), pX - text.Length * _hudFont.SizeInPoints / 2, pY - _hudFont.SizeInPoints / 2);
            if (centerX) pX -= text.Length * _hudFont.SizeInPoints / 2;
            if (centerY) pY -= _hudFont.SizeInPoints / 2;
            graphics.DrawString(text, _hudFont, new SolidBrush(Color.FromArgb(84, 67, 53)), pX, pY);
            //0xff544335
            alpha = 0.9f;
        }
    }
}