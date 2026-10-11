# Design

## Context

Unity 6000.6.5f1. Активных OpenSpec change нет: `core-movement`, `totoro-bounce` и `air-control` в архиве; воздух влит в `openspec/specs/core-movement/spec.md`. См. `proposal.md`, раздел Why.

Игрок — Prefab с `Rigidbody2D` и капсулой. Отскок — trigger на животе и `PlayerVerticalImpulse`. Системы эффектов сбора, UI и save нет. В `Assets/Art` уже лежат `Leaf.png`, `Seed.png`, `Nut.png` и другие предметы (PPU 128, pivot у летающих — центр).

## Goals / Non-Goals

**Goals:**

- Prefab предмета с одноразовым сбором по trigger.
- Счётчик сессии сцены, видимый разработчику в Play.
- Разделение: предмет / маркер сборщика / учёт.
- Несколько экземпляров в `Clearing` для проверки.

**Non-Goals:**

- Инвентарь, магазин, квесты, экономика, достижения.
- Save между запусками игры, `DontDestroyOnLoad` для счётчика.
- Полноценный UI, VFX-пайплайн, звук.
- Правки `PlayerMovement`, `PlayerJump`, `PlayerVerticalImpulse`, `TotoroBounce`, `CameraController`.
- GameManager и сторонние пакеты.

## Decisions

- Три маленьких MonoBehaviour, без общего менеджера сцены:
  1. `Collectible` на Prefab предмета — trigger, флаг «уже собран», исчезновение, запрос сбора.
  2. `CollectibleCollector` на префабе игрока — маркер «это тело может собирать». Не знает про бег и Тоторо. Держит ссылку на счётчик сцены или находит его один раз в `Awake`.
  3. `CollectibleTally` на объекте сцены — целочисленный счёт, инкремент, поле видно в Inspector в Play; опционально `Debug.Log` по флагу Inspector.
- Поток: `Collectible.OnTriggerEnter2D` → `other.GetComponent<CollectibleCollector>()` → если есть и предмет ещё не собран → пометить собранным, отключить trigger и объект (или только рендер+коллайдер), вызвать `tally.Add(1)`. Повторный Enter не проходит из‑за флага и отключённого коллайдера.
- Физика: у предмета `CircleCollider2D` (или box) с `isTrigger = true`, без `Rigidbody2D` — статичный trigger. У игрока уже есть dynamic `Rigidbody2D`, сообщений trigger достаточно. Слой не `Ground`, чтобы луч опоры предмет не считал землёй. Маску слоёв не усложняем, пока Default хватает.
- Prefab `Assets/Prefabs/Collectible.prefab`: `SpriteRenderer` + trigger + `Collectible`. Спрайт по умолчанию — один из готовых (`Leaf` / `Seed` / `Nut`), на экземплярах можно сменить в Inspector. Отдельный Prefab на каждый вид не обязателен в этом change.
- Исчезновение: `gameObject.SetActive(false)` после сбора. Анимации нет — системы эффектов в проекте нет.
- Жизненный цикл: `CollectibleTally` и экземпляры живут в сцене. Перезагрузка сцены обнуляет счёт и возвращает предметы. `DontDestroyOnLoad` не использовать.
- Интеграция с игроком: только добавить `CollectibleCollector` на существующий Prefab. Скрипты движения и bounce не менять.
- Проверка в разработке: `[SerializeField] int collectedCount` на `CollectibleTally` обновляется в Play и читается в Inspector; флаг лога в Console — по желанию. Отдельный HUD не делаем.

## Risks / Trade-offs

- [Trigger не срабатывает без Rigidbody на одной из сторон] → Rigidbody уже на игроке; предмет остаётся trigger без тела.
- [Предмет на слое Ground ломает луч опоры] → слой Default, не Ground.
- [Find счётчика каждый кадр] → поиск один раз в `Awake` у collector или сериализованное поле на объекте сцены у экземпляров; не на префабе игрока ссылку на сценный объект жёстко зашивать, если ломает Prefab — тогда Find в `Awake`.
- [Два коллайдера игрока дадут двойной Enter] → флаг `collected` до инкремента; повторный Add блокируется.
- [Путаница с trigger живота Тоторо] → разные компоненты; collectible ищет только `CollectibleCollector`, bounce — только импульс.
- [Счётчик как скрытый GameManager] → только инкремент и чтение count, без загрузки сцен и синглтона на всю игру.

## Migration Plan

Откат — удалить Prefab и скрипты collectibles, убрать `CollectibleCollector` с игрока, убрать `CollectibleTally` и экземпляры из `Clearing`. Движение и bounce остаются.
