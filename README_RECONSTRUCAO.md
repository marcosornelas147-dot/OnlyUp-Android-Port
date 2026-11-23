# Only Up Alpha - reconstrução Android (protótipo)

Esta é uma base Unity reconstruída, não o código-fonte original.

## Recriado
- Controle de personagem com CharacterController, gravidade e pulo.
- Câmera em terceira pessoa com controle por toque.
- Joystick virtual e botão de pulo.
- Percurso vertical procedural temporário com 64 plataformas e marcadores.
- Menu do Editor: Only Up > Rebuild Prototype Scene. Configura Android ARMv7/IL2CPP.

## Como abrir
1. Unity Hub + Unity 2021.3.45f1.
2. Instalar Android Build Support, Android SDK & NDK Tools e OpenJDK.
3. Abrir esta pasta como projeto Unity.
4. Criar/abrir uma cena vazia e executar o menu Only Up > Rebuild Prototype Scene.
5. File > Build Settings > Android > Switch Platform > Build.

## Limitações
O executável Windows e os arquivos *_Data não equivalem a um projeto Unity editável. Cena, modelos, animações, materiais e áudio originais ainda precisam ser extraídos/reconstruídos e integrados. O percurso procedural é temporário e não representa o mapa original.
