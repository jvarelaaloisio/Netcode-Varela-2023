using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace Sprites.Editor
{
    public class SpriteSlicerWindow : EditorWindow
    {
        private const string WindowTitle = "Sprite batch slicer";
        private static readonly Vector2 WindowSize = new(275, 85);
        private Vector2Int _sliceSize = new(32, 32);

        [MenuItem("Tools/" + WindowTitle)]
        [MenuItem("Assets/" + WindowTitle)]
        private static void OpenWindow()
        {
            var win = GetWindow<SpriteSlicerWindow>();
            win.titleContent = new GUIContent(WindowTitle);
            win.minSize = WindowSize;
            win.maxSize = WindowSize;
            win.Show();
        }

        [MenuItem("Assets/" + WindowTitle, true)]
        private static bool OpenWindowFromAssetsView_Validate()
        {
            return GetSelectedTextures().Length > 0;
        }
        
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Size", GUILayout.ExpandWidth(false));
            _sliceSize = EditorGUILayout.Vector2IntField(GUIContent.none, _sliceSize, GUILayout.ExpandWidth(false));
            if (GUILayout.Button(new GUIContent(string.Empty, EditorGUIUtility.IconContent("_Help").image),
                    GUILayout.ExpandWidth(false))
                || (Event.current.isKey && Event.current.keyCode == KeyCode.F1))
            {
                Application.OpenURL("https://github.com/jvarelaaloisio");
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Slice") || (Event.current.isKey && Event.current.keyCode == KeyCode.Return))
            {
                var textures = GetSelectedTextures();
                SliceSprites(textures, _sliceSize);
            }
            GUILayout.Space(5);
            DrawHorizontalLine(5, new Color(.15f, .15f, .15f));
            GUILayout.Space(5);
            if (GUILayout.Button("Slice horizontally"))
            {
                var textures = GetSelectedTextures();
                SliceSpritesHorizontally(textures);
            }
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draws a horizontal-line styled rect
        /// </summary>
        /// <param name="height"></param>
        /// <param name="color"></param>
        private static void DrawHorizontalLine(int height, Color color)
        {
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(GUILayoutUtility.GetLastRect().width, height), color);
        }

        /// <summary>
        /// Returns the texture2D assets that are being selected.
        /// </summary>
        /// <returns></returns>
        private static Texture2D[] GetSelectedTextures()
        {
            var selection = Selection.objects;
            var textures = selection.OfType<Texture2D>().ToArray();
            return textures;
        }

        /// <summary>
        /// Slices the given textures using the given size
        /// </summary>
        /// <param name="textures"></param>
        /// <param name="size"></param>
        public static void SliceSprites(Texture2D[] textures, Vector2Int size)
        {
            if (!textures.Any())
            {
                LogNoAssetsSelected();
                return;
            }
            Debug.Log($"{WindowTitle}: <color=yellow>Slicing...</color>");
            var factory = new SpriteDataProviderFactories();
            factory.Init();
            var changeCount = 0;
            var skippedTextures = string.Empty;
            var modifiedTextures = string.Empty;
            foreach (var texture in textures)
            {
                if (size.x > texture.width
                    || size.y > texture.height
                    || (size.x * 2 > texture.width && size.y * 2 > texture.height))
                {
                    skippedTextures += $" ({texture.name})";
                    continue;
                }
                string path = AssetDatabase.GetAssetPath(texture);
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                
                textureImporter.spriteImportMode = SpriteImportMode.Single;
                textureImporter.isReadable = true;
                textureImporter.spriteImportMode = SpriteImportMode.Multiple;
                
                var dataProvider = factory.GetSpriteEditorDataProviderFromObject(textureImporter);
                dataProvider.InitSpriteEditorDataProvider();
                
                var spriteRects = new List<SpriteRect>();
                for (int x = 0; x < texture.width; x += size.x)
                {
                    for (int y = texture.height; y > 0; y -= size.y)
                    {
                        
                        var rect = new SpriteRect()
                        {
                            pivot = new Vector2(0.5f, 0.5f),
                            spriteID = GUID.Generate(),
                            name = (texture.height - y) / size.y + ", " + x / size.x,
                            rect = new Rect(x, y - size.y, size.x, size.y)
                        };
                        
                        spriteRects.Add(rect);
                    }
                }
                dataProvider.SetSpriteRects(spriteRects.ToArray());

                dataProvider.Apply();
                textureImporter.SaveAndReimport();
                
                modifiedTextures += $" ({texture.name})";
                changeCount++;
            }
            Debug.Log($"{WindowTitle}: <color=green>Done Slicing!</color>" +
                      $"\n<color=black>Assets modified:</color> {changeCount}" +
                      $"\n{modifiedTextures}" +
                      $"\n------------------------------------------------------" +
                      $"\n<color=black>Skipped:</color> {skippedTextures}");
        }
        
        public static void SliceSpritesHorizontally(Texture2D[] textures)
        {
            if (!textures.Any())
            {
                LogNoAssetsSelected();
                return;
            }
            Debug.Log($"{WindowTitle}: <color=yellow>Slicing...</color>");
            var factory = new SpriteDataProviderFactories();
            factory.Init();
            var changeCount = 0;
            var skippedTextures = string.Empty;
            var modifiedTextures = string.Empty;
            foreach (var texture in textures)
            {
                var size = texture.height;
                if (size * 2 > texture.width)
                {
                    skippedTextures += $" ({texture.name})";
                    continue;
                }
                string path = AssetDatabase.GetAssetPath(texture);
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                
                textureImporter.spriteImportMode = SpriteImportMode.Single;
                textureImporter.isReadable = true;
                textureImporter.spriteImportMode = SpriteImportMode.Multiple;
                
                var dataProvider = factory.GetSpriteEditorDataProviderFromObject(textureImporter);
                dataProvider.InitSpriteEditorDataProvider();
                
                var spriteRects = new List<SpriteRect>();
                for (int x = 0; x < texture.width; x += size)
                {
                    for (int y = texture.height; y > 0; y -= size)
                    {
                        
                        var rect = new SpriteRect()
                        {
                            pivot = new Vector2(0.5f, 0.5f),
                            spriteID = GUID.Generate(),
                            name = (size - y) / size + ", " + x / size,
                            rect = new Rect(x, y - size, size, size)
                        };
                        
                        spriteRects.Add(rect);
                    }
                }
                dataProvider.SetSpriteRects(spriteRects.ToArray());

                dataProvider.Apply();
                textureImporter.SaveAndReimport();
                
                modifiedTextures += $" ({texture.name})";
                changeCount++;
            }
            Debug.Log($"{WindowTitle}: <color=green>Done Slicing!</color>" +
                      $"\n<color=black>Assets modified:</color> {changeCount}" +
                      $"\n{modifiedTextures}" +
                      $"\n------------------------------------------------------" +
                      $"\n<color=black>Skipped:</color> {skippedTextures}");
        }

        private static void LogNoAssetsSelected()
        {
            Debug.LogError($"{WindowTitle}: <color=orange>No texture assets are selected!</color>");
        }
    }
}