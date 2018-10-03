if ("serviceWorker" in navigator) {
    navigator.serviceWorker.register("/sw.js?v=1");
}

﻿Vue.component("clash-clan-info",
    {
        props: ["info"],
        template: `
<div class="block about">
    <div>
        <img v-bind:src="info.badgeUrls.small" alt="">
    </div>
    <div>
        <div>Всего очков: {{info.clanPoints}} / {{info.clanVersusPoints}}</div>
        <div>Серия побед: {{info.warWinStreak}}</div>
        <div>Выиграно войн: {{info.warWins}}</div>
        <div>Проиграно войн: {{info.WarLosses}}</div>
        <div>Ничья: {{info.warTies}}</div>
        <div>Побед &divide; поражений: {{(info.warWins / info.warLosses).toFixed(3)}}</div>
        <div>Публичный варлог: {{info.isWarLogPublic ? "Да" : "Нет"}}</div>
        <div>Необходимо трофеев: {{info.requiredTrophies}}</div>
        <div>Расположение клана: {{info.location.name}}</div>
        <div>Соклановцы: {{info.members}}/50</div>
    </div>
</div>

`
    });

Vue.component("clash-clan-moto",
    {
        data: function () {
            return {
                count: 0
            }
        },
        template: '<a>!!!!!</a>'
    });

Vue.component("clash-clan-donation",
    {
        data: function () {
            return {
                count: 0
            }
        },
        template: '<a>!!!!!</a>'
    });

Vue.component("clash-warlog",
    {
        data: function () {
            return {
                count: 0
            }
        },
        template: '<a>!!!</a>'
    });

var app = new Vue({
    el: "#app",
    created() {
        this.fetchData();
    },
    data() {
        return {
            post: {tag:"123"}
        }
    },
    methods: {
        fetchData() {
            fetch("api/clash").then(r => r.json().then(t => this.post = t));
        }
    },
    template: `
<clash-clan-info v-bind:info="post"></clash-clan-info>
`
})