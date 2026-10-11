# Spec Delta

## Purpose

Задаёт сквозные требования уровня «древнее дерево»: цель, цикл, завершение и ограничения прототипа. Детали зон — в capability соответствующих zone-change.

## ADDED Requirements

### Requirement: Player goal restore tree light
Уровень MUST сообщать игроку через окружение и финал, что цель — **вернуть свет на верхушку древнего дерева**, без обязательных текстовых tutorial-экранов.

#### Scenario: Finale communicates restored light
- **WHEN** игрок завершает зону 5 по правилам `zone-tree-crown`
- **THEN** игрок видит последовательное зажигание огоньков и изменение освещения леса как результат своих действий

### Requirement: Core exploration bounce loop
Прохождение MUST опираться на цикл исследования, преграды, отскока/прыжка и реакции окружения, используя существующие `core-movement` и `totoro-bounce` без замены их контрактов.

#### Scenario: Bounce remains central after zone 1
- **WHEN** игрок прошёл обучение в зоне 1
- **THEN** последующие зоны по-прежнему требуют вертикального импульса и руления в воздухе там, где геометрия не проходима только с земли

### Requirement: Five sequential zones in one scene
Весь маршрут MUST умещаться в **одной** playable-сцене с пятью зонами в порядке 1→5; отдельные zone-change MUST NOT требовать второй сцены в билде.

#### Scenario: Single build scene contains route
- **WHEN** игрок запускает игру из EditorBuildSettings
- **THEN** доступен непрерывный путь от старта зоны 1 до финала зоны 5 без загрузки другой сцены

### Requirement: Prototype scope boundaries
Уровень MUST NOT вводить бой, инвентарь между сессиями, сложную прогрессию или обязательный сбор 100% collectibles для победы.

#### Scenario: Win without full collection
- **WHEN** игрок достигает гнезда с минимально необходимым числом семян, заданным в `zone-tree-crown`
- **THEN** условие завершения выполняется даже если редкие семена с альтернативных маршрутов не собраны

### Requirement: Zone changes ownership
Каждая зона MUST быть описана и реализована отдельным OpenSpec change; umbrella MUST NOT дублировать детальные acceptance-критерии зон.

#### Scenario: Zone spec is authoritative for geometry
- **WHEN** возникает конфликт между umbrella и `zone-*` spec по расстановке объектов зоны
- **THEN** приоритет у spec соответствующей зоны
